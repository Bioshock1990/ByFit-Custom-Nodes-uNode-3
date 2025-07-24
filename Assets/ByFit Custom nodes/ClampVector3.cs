using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
    [NodeMenu("ByFit Custom nodes/Math", "Clamp Vector3",
        tooltip = "Clamps each component of the Vector3 between min and max Vector3 components.",
        icon = typeof(Mathf))]
    [Description("ClampVector3: Returns a Vector3 where each component is clamped between corresponding min and max values.")]
    public class ClampVector3Node : IStaticNode {
        [Input]
        public static Vector3 value = Vector3.zero;

        [Input]
        public static Vector3 min = Vector3.zero;

        [Input]
        public static Vector3 max = Vector3.one;

        [Output]
        public static Vector3 ClampVector3(Vector3 value, Vector3 min, Vector3 max) {
            float x = Mathf.Clamp(value.x, min.x, max.x);
            float y = Mathf.Clamp(value.y, min.y, max.y);
            float z = Mathf.Clamp(value.z, min.z, max.z);
            return new Vector3(x, y, z);
        }
    }
}