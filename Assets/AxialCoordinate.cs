using UnityEngine;

public struct AxialCoordinate 
{
    public int Q { get; }
    public int R { get; }

    public int S => -(Q + R);

    public static readonly float SQRT3 = Mathf.Sqrt(3);

    public Vector3 ToWorldPosition => new Vector3(
        3f / 2f * Q,
        0,
        -SQRT3 / 2f * Q - SQRT3 * R
        );

    public static readonly AxialCoordinate Zero = new AxialCoordinate(0, 0);

    public AxialCoordinate(int q, int r)
    {
        Q = q;
        R = r;
    }

    public static readonly AxialCoordinate[] NeighborDirections = {
    new AxialCoordinate(1, 0),    // East
    new AxialCoordinate(1, -1),   // North-East
    new AxialCoordinate(0, -1),   // North-West
    new AxialCoordinate(-1, 0),   // West
    new AxialCoordinate(-1, 1),   // South-West
    new AxialCoordinate(0, 1)     // South-East
};

    public static AxialCoordinate CubeToAxial(Vector3Int cubeCoordinates)
    {
        int q = cubeCoordinates.x;
        int r = cubeCoordinates.z;
        return new AxialCoordinate(q, r);
    }

    public static Vector3Int AxialToCube(AxialCoordinate axial)
    {
        int x = axial.Q;
        int z = axial.R;
        int y = -x - z;
        return new Vector3Int(x, y, z);
    }
}
