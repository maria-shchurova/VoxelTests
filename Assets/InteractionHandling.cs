using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandling : MonoBehaviour
{
	[SerializeField]
	private HexGrid hexGrid;

	[SerializeField]
	private HexMesh mesh;

	void Start()
    {
        
    }

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput();
		}

		if (Input.GetKeyDown(KeyCode.T))
		{
			mesh.Triangulate(hexGrid.cells);
		}

	}

	void HandleInput()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
            if (GetCellAtWorldPosition(hit.point) != null)
            {
                Debug.Log("Clicked on cell " + GetCellAtWorldPosition(hit.point).name);
            }
            else
            {
                Debug.Log("could not get closest cell at hit point");
            }
        }
	}

	HexCell GetCellAtWorldPosition(Vector3 worldPosition)
	{
		HexCell closestCell = null;
		float closestDistanceSqr = float.MaxValue; // Initialize to a large number

		foreach (var kvp in hexGrid.cellsByCoordinates)
		{
			Vector3 cellCenter = kvp.Key;
			float distanceSqr = (worldPosition - cellCenter).sqrMagnitude; // Use squared magnitude to avoid square root calculation

			if (distanceSqr < closestDistanceSqr)
			{
				closestDistanceSqr = distanceSqr;
				closestCell = kvp.Value;
			}
		}

		return closestCell;
	}

	void TouchCell(Vector3 position)
	{
		//position = transform.InverseTransformPoint(position);
		Debug.Log("touched at " + position);
	}

}
