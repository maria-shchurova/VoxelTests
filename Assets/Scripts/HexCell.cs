using UnityEngine;

public class HexCell 
{
    public Vector3 position;
    public bool isActive;
    public CellType type; 

    public HexCell[] neighbors;
    public HexCell neighborUp;
    public HexCell neighborDown;

    public HexCell(CellType type, bool isActive = true)
    {
        this.isActive = isActive;
        this.type = type;
        neighbors = new HexCell[6]; 
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