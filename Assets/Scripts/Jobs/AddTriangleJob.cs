using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace Jobs
{
    [BurstCompile]
    public struct AddTriangleJob : IJob
    {
        private int vertexIndex;
        private int triangleIndex;

        public NativeArray<float3> vertices;
        public NativeArray<int> triangles;

        [ReadOnly] public NativeArray<float3> hexMetricsCorners;
        [ReadOnly] public NativeArray<BitmaskNeighbors> bitmasks;
        [ReadOnly] public NativeArray<float3> cellsCentersCoordinates;

        public void Execute()
        {
            vertexIndex = 0;
            triangleIndex = 0;


            for (int i = 0; i < cellsCentersCoordinates.Length; i++)
            {
                Triangulate(cellsCentersCoordinates[i], bitmasks[i]);            
            }

        }
        private void Triangulate(float3 cellCenter, BitmaskNeighbors bitmask)
        {
            float3 cellCenterTop = cellCenter + new float3(0, 1, 0);

            for (int i = 0; i < 6; i++)
            {

                if ((bitmask & GetDirectionBitmask(6)) == 0)
                {
                    // Top face
                    AddTriangle(
                        cellCenterTop,
                        cellCenterTop + hexMetricsCorners[i],
                        cellCenterTop + hexMetricsCorners[i + 1]
                    );
                }

                if ((bitmask & GetDirectionBitmask(7)) == 0)
                {
                    // Bottom face
                    AddTriangle(
                        cellCenter + hexMetricsCorners[i + 1],
                        cellCenter + hexMetricsCorners[i],
                        cellCenter
                    );
                }

                // Check if the neighbor in this direction is inactive or absent using the bitmask
                if ((bitmask & GetDirectionBitmask(i)) == 0)
                {
                    // Side face 1
                    AddTriangle(
                        cellCenterTop + hexMetricsCorners[i + 1],
                        cellCenterTop + hexMetricsCorners[i],
                        cellCenter + hexMetricsCorners[i]
                    );

                    // Side face 2
                    AddTriangle(
                        cellCenter + hexMetricsCorners[i + 1],
                        cellCenterTop + hexMetricsCorners[i + 1],
                        cellCenter + hexMetricsCorners[i]
                    );
                }

            }
        }


        void AddTriangle(float3 v1, float3 v2, float3 v3)
        {
            vertices[vertexIndex] = v1;
            vertices[vertexIndex + 1] = v2;
            vertices[vertexIndex + 2] = v3;

            triangles[triangleIndex] = vertexIndex;
            triangles[triangleIndex + 1] = vertexIndex + 1;
            triangles[triangleIndex + 2] = vertexIndex + 2;

            vertexIndex += 3;
            triangleIndex += 3;
        }


        private BitmaskNeighbors GetDirectionBitmask(int i)
        {
            switch (i)
            {
                case 0: return BitmaskNeighbors.NE;
                case 1: return BitmaskNeighbors.E;
                case 2: return BitmaskNeighbors.SE;
                case 3: return BitmaskNeighbors.SW;
                case 4: return BitmaskNeighbors.W;
                case 5: return BitmaskNeighbors.NW;
                case 6: return BitmaskNeighbors.TOP;
                case 7: return BitmaskNeighbors.BOTTOM;
                default: return BitmaskNeighbors.None; // Shouldn't reach here
            }
        }
    }
}
