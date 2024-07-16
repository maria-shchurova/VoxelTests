using UnityEngine;
public class HexGrid : MonoBehaviour
{
	//public int width = 6;
	//public int height = 6;
	//public int depth = 6;
	public int size;

	public HexCell cellPrefab;

	HexCell[] cells;
	HexCell[,,] cashedCells;

	HexMesh hexMesh;
	MeshCollider meshCollider;

	void Awake()
	{
		hexMesh = GetComponentInChildren<HexMesh>();
		meshCollider = gameObject.AddComponent<MeshCollider>();

		cells = new HexCell[size * size * size];
		cashedCells = new HexCell[size, size, size];

		for (int x = 0, i = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				for (int z = 0; z < size; z++)
				{
					CreateCell(x, y, z, i++);
				}
			}
		}
	}

    private void Start()
    {
		hexMesh.Triangulate(cells);
		meshCollider.sharedMesh = hexMesh.GetComponent<MeshFilter>().mesh;
	}


	void CreateCell(int x, int y,int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cashedCells[x, y, z] = cell;

		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
	}

	private bool IsFaceVisible(int x, int y, int z)
	{
		// Check if the neighboring voxel in the given direction is inactive or out of bounds
		if (x < 0 || x >= size || y < 0 || y >= size || z < 0 || z >= size)
			return true; // Face is at the boundary of the chunk

		return !cashedCells[x, z, y].isActive; 
	}
}
