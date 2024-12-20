using System;
using System.Linq;
using Assets.Scripts.Jobs;
using Jobs;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
	public class HexMesh : MonoBehaviour
	{
		private NativeArray<float3> vertices;
		private NativeArray<int> triangles;

		private NativeArray<float3> hexMetricsCorners;
		private NativeArray<BitmaskNeighbors> cellsBitmasks;
		private NativeArray<float3> cellsCentersCoordinates;

		[SerializeField]
		private MeshFilter hexMeshFilter;

		public void Initialize()
		{
			int chunkSize = World.Instance.chunkSize;

			hexMeshFilter.mesh = new Mesh();
			hexMeshFilter.mesh.name = "Hex Mesh";

			// Predefine sizes (estimate based on grid size)
			int maxVertices = chunkSize * chunkSize * chunkSize * 12;
			int maxTriangles = maxVertices * 3; // Estimate based on the number of faces

			vertices = new NativeArray<float3>(maxVertices, Allocator.Persistent);
			triangles = new NativeArray<int>(maxTriangles, Allocator.Persistent);

			hexMetricsCorners = HexMetrics.corners.ToNativeArrayFloat3(Allocator.Persistent);
		}

		private void Clear()
		{
			hexMeshFilter.mesh.Clear(false);
			
			// Dispose of existing NativeArrays before creating new ones
			// if (vertices.IsCreated) vertices.Dispose();
			// if (triangles.IsCreated) triangles.Dispose();
			var clearTrianglesJob = new ClearIntArrayJob { Array = triangles };
			clearTrianglesJob.Schedule(triangles.Length, 64).Complete();
			
			var clearVerticesJob = new ClearFloat3ArrayJob(){ Array = vertices };
			clearVerticesJob.Schedule(vertices.Length, 64).Complete();
			
			// // Reset the arrays
			// vertices = new NativeArray<float3>(vertices.Length, Allocator.Persistent);
			// triangles = new NativeArray<int>(triangles.Length, Allocator.Persistent);
		}

		public void Triangulate(HexCell[] cells)
		{
			Clear();

			var activeCellPositionsList = cells.Where(c => c.isActive).Select(c => c.position).ToArray();
			var bitmasksList = cells.Where(c => c.isActive).Select(c => c.neighborsBitmask).ToArray();

			cellsCentersCoordinates = activeCellPositionsList.ToNativeArrayFloat3(Allocator.Persistent);
			cellsBitmasks = new NativeArray<BitmaskNeighbors>(cellsCentersCoordinates.Length, Allocator.Persistent);

			for(int i = 0; i < cellsCentersCoordinates.Length; i++)
			{
				cellsBitmasks[i] = bitmasksList[i];
			}

			AddTriangleJob job = new AddTriangleJob
			{
				vertices = vertices,
				triangles = triangles,
				hexMetricsCorners = hexMetricsCorners,
				cellsCentersCoordinates = cellsCentersCoordinates,
				bitmasks = cellsBitmasks
			};
			JobHandle jobHandle = job.Schedule();
			jobHandle.Complete();

			hexMeshFilter.mesh.vertices = vertices.ToVector3Array();
			hexMeshFilter.mesh.triangles = triangles.ToIntArray();
			hexMeshFilter.mesh.RecalculateNormals();
		}

		public void Dispose()
		{
			hexMetricsCorners.Dispose();
			cellsCentersCoordinates.Dispose();
			cellsBitmasks.Dispose();
		}

		private void OnDestroy()
		{
			try
			{
				Dispose();
			}
			catch (ObjectDisposedException e)
			{
				//Debug.LogWarning(e);
			}
		}
	}
}
