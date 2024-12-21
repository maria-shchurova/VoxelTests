using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class InteractionHandling : MonoBehaviour
{
	[SerializeField]
	private BuildingManager modeManager;

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			HandleInput();
		}
	}

	void HandleInput()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		//first find the chunk:
		
		
		// if (Physics.Raycast(ray, out RaycastHit hit))
		// {
  //           if (GetCellAtWorldPosition(hit.point) != null)
  //           {
		// 		TouchCell(GetCellAtWorldPosition(hit.point));
  //           }
  //           else
  //           {
  //               Debug.Log("could not get closest cell at hit point");
  //           }
  //       }
	}

	HexCell GetCellAtWorldPosition(Vector3 worldPosition, HexChunk hexGrid)
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

	void TouchCell(HexCell cell, HexChunk chunk)
	{
		if(modeManager.currentMode == BuildingMode.ADD)
        {
			cell.isActive = true;
		}

		if (modeManager.currentMode == BuildingMode.REMOVE)
		{
			cell.isActive = false;
		}
		
		chunk.Recreate();
	}

}
