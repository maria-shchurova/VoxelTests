using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandling : MonoBehaviour
{
    // Start is called before the first frame update
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
			Debug.Log("Clicked on cell with coordinates: " + axialCoord.Q + ", " + axialCoord.R);
		}
	}

	void TouchCell(Vector3 position)
	{
		//position = transform.InverseTransformPoint(position);
		Debug.Log("touched at " + position);
	}

}
