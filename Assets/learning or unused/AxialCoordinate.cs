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

    public static AxialCoordinate FromWorldPosition(Vector3 position)
    {
        float q = (2f / 3f * position.x) / HexMetrics.outerRadius;
        float r = (-1f / 3f * position.x + Mathf.Sqrt(3) / 3f * position.z) / HexMetrics.outerRadius;
        return RoundToAxial(q, r);
    }

    public static readonly AxialCoordinate Zero = new AxialCoordinate(0, 0);

    public AxialCoordinate(int q, int r)
    {
        Q = q;
        R = r;
    }

    public readonly AxialCoordinate ApplyDirection(AxialAxes axes, int delta)
    {
        switch (axes)
        {
            case AxialAxes.QS:
                return new AxialCoordinate(Q + delta, R);
            case AxialAxes.RS:
                return new AxialCoordinate(Q, R + delta);
            case AxialAxes.QR:
                return new AxialCoordinate(Q + delta, R - delta);
            default:
                throw new System.Exception();
        }
    }


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
    private static AxialCoordinate RoundToAxial(float q, float r)
    {
        int qInt = Mathf.RoundToInt(q);
        int rInt = Mathf.RoundToInt(r);
        int sInt = Mathf.RoundToInt(-q - r);

        float qDiff = Mathf.Abs(qInt - q);
        float rDiff = Mathf.Abs(rInt - r);
        float sDiff = Mathf.Abs(sInt + q + r);

        if (qDiff > rDiff && qDiff > sDiff)
        {
            qInt = -rInt - sInt;
        }
        else if (rDiff > sDiff)
        {
            rInt = -qInt - sInt;
        }

        return new AxialCoordinate(qInt, rInt);
    }
}
