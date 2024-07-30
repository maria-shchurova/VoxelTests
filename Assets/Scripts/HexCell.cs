using UnityEngine;

public class HexCell : MonoBehaviour
{
    public AxialCoordinate coordinates;
    public bool isActive;
    public CellType type; 

    [SerializeField]
    HexCell[] neighbors;
    
    public HexCell(CellType type, bool isActive = true)
    {
        this.isActive = isActive;
        this.type = type;
    }

    public HexCell GetNeighbor(int index)
    {
        return neighbors[index];
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

    public enum CellType
    {
        Air,    // Represents empty space
        Grass,  // Represents grass block
        Stone,  // Represents stone block
                // Add more types as needed
    }
}