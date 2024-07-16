using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HexGrid : MonoBehaviour
{
	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;

	HexCell[] cells;

	private Dictionary<HexCoordinates, HexCell> CellByCoordinates;
	public Text cellLabelPrefab;
	Canvas gridCanvas;

	HexMesh hexMesh;
	MeshCollider meshCollider;

	public HexagonPrismMesh prismPrefab;

	void Awake()
	{
		gridCanvas = GetComponentInChildren<Canvas>();
		hexMesh = GetComponentInChildren<HexMesh>();
		meshCollider = gameObject.AddComponent<MeshCollider>();

		cells = new HexCell[height * width];
		CellByCoordinates = new Dictionary<HexCoordinates, HexCell>();

		for (int z = 0, i = 0; z < height; z++)
		{
			for (int x = 0; x < width; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}

    private void Start()
    {
		hexMesh.Triangulate(cells);
		meshCollider.sharedMesh = hexMesh.GetComponent<MeshFilter>().mesh;
	}

	void Update()
	{
		if (Input.GetMouseButton(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit))
		{
			TouchCell(hit.point);
		}
	}

	void TouchCell(Vector3 position)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);

		Instantiate<HexagonPrismMesh>(prismPrefab, HexCoordinates.ToPosition(coordinates), Quaternion.identity);
		//hexMesh.Triangulate3D(GetCellAtCoordinates(coordinates));
		Debug.Log("touched at " + coordinates.ToString());
	}


	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		CellByCoordinates.Add(cell.coordinates, cell);

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();

	}

	HexCell GetCellAtCoordinates(HexCoordinates cellCoordinates)
    {
		return CellByCoordinates[cellCoordinates];
    }
}
