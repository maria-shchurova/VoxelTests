using UnityEngine;

public class HexCell 
{
    public Vector3 position;
    public bool isActive;
    public CellType type; 

    public HexCell[] neighbors;
    public HexCell neighborUp;
    public HexCell neighborDown;

    public BitmaskNeighbors neighborsBitmask;

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

    public void SetBitmaskNeighbor(BitmaskNeighbors flag, HexCell cell)
    {
        if(cell.isActive)
        {
            FlagsHelper.Set(ref neighborsBitmask, flag);
        }
        if(isActive)
        {
            FlagsHelper.Set(ref cell.neighborsBitmask, Opposite(flag));
        }
    }

    public enum CellType
    {
        Air,    // Represents empty space
        Grass,  // Represents grass block
        Stone,  // Represents stone block
                // Add more types as needed
    }

    private BitmaskNeighbors Opposite(BitmaskNeighbors neighbor)
    {
        switch (neighbor)
        {
            case BitmaskNeighbors.NE:
                return BitmaskNeighbors.SW;
                break;
            case BitmaskNeighbors.E:
                return BitmaskNeighbors.W;
                break;
            case BitmaskNeighbors.SE:
                return BitmaskNeighbors.NW;
                break;
            case BitmaskNeighbors.SW:
                return BitmaskNeighbors.NE;
                break;
            case BitmaskNeighbors.W:
                return BitmaskNeighbors.E;
                break;
            case BitmaskNeighbors.NW:
                return BitmaskNeighbors.SE;
                break;
            case BitmaskNeighbors.TOP:
                return BitmaskNeighbors.BOTTOM;
                break;
            case BitmaskNeighbors.BOTTOM:
                return BitmaskNeighbors.TOP;
                break;

            default:
                return BitmaskNeighbors.None;

        }
    }
}