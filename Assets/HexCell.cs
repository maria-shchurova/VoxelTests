using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public Vector3 position;
    public Color color;
    public bool isActive;

    [SerializeField]
    HexCell[] neighbors;

    public HexCell(Vector3 position, Color color, bool isActive = true)
    {
        this.position = position;
        this.color = color;
        this.isActive = isActive;
    }

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }
}