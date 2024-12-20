using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class InteractionHandling : MonoBehaviour
{
	[SerializeField]
	private HexChunk hexGrid;

	[SerializeField]
	private HexMesh mesh;

	[SerializeField]
	private BuildingManager modeManager;

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
				TouchCell(GetCellAtWorldPosition(hit.point));
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

	void TouchCell(HexCell cell)
	{
		if(modeManager.currentMode == BuildingMode.ADD)
        {
			cell.isActive = true;
		}

		if (modeManager.currentMode == BuildingMode.REMOVE)
		{
			cell.isActive = false;
		}

		mesh.Triangulate(hexGrid.cells);
	}

}
