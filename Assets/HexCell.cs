using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Vector3 position;
    public Color color;
    public bool isActive;

    public HexCell(Vector3 position, Color color, bool isActive = true)
    {
        this.position = position;
        this.color = color;
        this.isActive = isActive;
    }
}