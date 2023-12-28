using UnityEngine;

namespace BattlefieldSimulator
{
    public static class HexMetrics
    {
        public const float outerRadius = 1f;

        public const float innerRadius = outerRadius * 0.866f;

        public static Vector3[] corners = {
        new Vector3(0f, 0f, outerRadius),
        new Vector3(innerRadius, 0f, 0.5f * outerRadius),
        new Vector3(innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(0f, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
        new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
        };
    }
}
