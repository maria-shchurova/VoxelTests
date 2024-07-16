using UnityEngine;
public class HexGrid : MonoBehaviour
{
	//public int width = 6;
	//public int height = 6;
	//public int depth = 6;
	public int size;

	public HexCell cellPrefab;

	HexCell[] cells;

	HexMesh hexMesh;
	MeshCollider meshCollider;

	void Awake()
	{
		hexMesh = GetComponentInChildren<HexMesh>();
		meshCollider = gameObject.AddComponent<MeshCollider>();

		cells = new HexCell[size * size * size];

		for (int z = 0, i = 0; z < size; z++)
		{
			for (int x = 0; x < size; x++)
			{
				for (int y = 0; y < size; y++)
				{
					CreateCell(x, z, y, i++);
				}
			}
		}
	}

    private void Start()
    {
		hexMesh.Triangulate(cells);
		meshCollider.sharedMesh = hexMesh.GetComponent<MeshFilter>().mesh;
	}


	void CreateCell(int x, int z,int y, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = y * HexMetrics.height;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
	}

	
}
