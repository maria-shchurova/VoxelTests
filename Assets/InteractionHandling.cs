using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandling : MonoBehaviour
{
	[SerializeField]
	private HexGrid hexGrid;

	void Start()
    {
        
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
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			Vector3 worldPosition = hit.point;
			AxialCoordinate axialCoord = AxialCoordinate.FromWorldPosition(worldPosition);

			HexCell touchedCell;
			if (hexGrid.cellsByAxialCoordinates.TryGetValue(axialCoord, out touchedCell))
            {
				//Debug.Log("Clicked on cell with coordinates: " + axialCoord.Q + ", " + axialCoord.R);
				Debug.Log("Clicked on cell " + touchedCell.name);
			}
            else
            {
				Debug.Log("could not get cell with coordinates: " + axialCoord.Q + ", " + axialCoord.R);
			}
		}
	}

	void TouchCell(Vector3 position)
	{
		//position = transform.InverseTransformPoint(position);
		Debug.Log("touched at " + position);
	}

}
