using UnityEngine;
using System.Collections.Generic;

public class HexCoordinates3D : MonoBehaviour
{
    public class Hex
    {
        public int Q; // Column
        public int R; // Row
        public int S; // Sum is always zero: Q + R + S = 0
        public int Z; // Height

        public Hex(int q, int r, int z)
        {
            this.Q = q;
            this.R = r;
            this.S = -q - r;
            this.Z = z;
        }
    }

    private Dictionary<Vector3, Hex> hexes = new Dictionary<Vector3, Hex>();

    public void AddHex(int q, int r, int z, GameObject hexGO)
    {
        Hex hex = new Hex(q, r, z);
        Vector3 position = hexGO.transform.position;
        hexes[position] = hex;
    }

    public Hex GetHexAt(Vector3 position)
    {
        return hexes.ContainsKey(position) ? hexes[position] : null;
    }
}