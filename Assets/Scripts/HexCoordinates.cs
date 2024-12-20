using UnityEngine;

namespace Assets.Scripts
{
	[System.Serializable]
	public struct HexCoordinates
	{

		public int X { get; private set; }

		public int Z { get; private set; }

		public int Y
		{
			get
			{
				return -X - Z;
			}
		}
		public HexCoordinates(int x, int z)
		{
			X = x;
			Z = z;
		}


		public static HexCoordinates FromPosition(Vector3 position)
		{
			float x = position.x / (HexMetrics.innerRadius * 2f);
			float y = -x;

			float offset = position.z / (HexMetrics.outerRadius * 3f);
			x -= offset;
			y -= offset;

			int iX = Mathf.RoundToInt(x);
			int iY = Mathf.RoundToInt(y);
			int iZ = Mathf.RoundToInt(-x - y);

			if (iX + iY + iZ != 0)
			{
				float dX = Mathf.Abs(x - iX);
				float dY = Mathf.Abs(y - iY);
				float dZ = Mathf.Abs(-x - y - iZ);

				if (dX > dY && dX > dZ)
				{
					iX = -iY - iZ;
				}
				else if (dZ > dY)
				{
					iZ = -iX - iY;
				}
			}

			return new HexCoordinates(iX, iZ);
		}

		public static Vector3 ToPosition(HexCoordinates hexCoordinates)
		{
			float x = (hexCoordinates.X + hexCoordinates.Z * 0.5f) * (HexMetrics.innerRadius * 2f);
			float z = hexCoordinates.Z * (HexMetrics.outerRadius * 1.5f);

			return new Vector3(x, 0f, z);
		}

		public override string ToString()
		{
			return "(" +
			       X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
		}

		public string ToStringOnSeparateLines()
		{
			return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
		}
	}
}