using UnityEngine;

public static class HexMetrics
{
	public const float outerRadius = 1f;

	public const float innerRadius = outerRadius * 0.866025404f;

	public const float height = outerRadius;

	public static Vector3[] corners = {
		//bottom
		new Vector3(0f, 0f, outerRadius),
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(0f, 0f, -outerRadius),
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
		new Vector3(0f, 0f, outerRadius) //"7th" corner which is actially the first, for mesh generation
	};
}
