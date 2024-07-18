using UnityEngine;

public class testAxises : MonoBehaviour
{
    public AxialCoordinate hex = new AxialCoordinate(0, 0);
    private Vector3 origin;

    public GameObject debugSphere;
    public int deltaQS;
    public int deltaRS;
    public int deltaQR;

    void Start()
    {
        origin = transform.position;
    }

    private void Update()
    {
        DrawNeighborDirections(hex);
    }

    void DrawNeighborDirections(AxialCoordinate hex)
    {
        AxialCoordinate neighbor = hex.ApplyDirection(AxialAxes.QS, deltaQS);
        AxialCoordinate neighbor2 = hex.ApplyDirection(AxialAxes.RS, deltaRS);
        AxialCoordinate neighbor3 = hex.ApplyDirection(AxialAxes.QR, deltaQR);

        Vector3 worldPosition = neighbor.ToWorldPosition;
        Vector3 worldPosition2 = neighbor2.ToWorldPosition;
        Vector3 worldPosition3 = neighbor3.ToWorldPosition;

        Debug.DrawRay(origin, worldPosition, Color.red);
        Debug.DrawRay(origin, worldPosition2, Color.red);
        Debug.DrawRay(origin, worldPosition3, Color.red);        
 
    }
}
