using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
    [NodeMenu("ByFit Custom nodes/Math", "Clamp Vector2",
        tooltip = "Clamps each component of the Vector2 between min and max Vector2 components.",
        icon = typeof(Mathf))]
    [Description("ClampVector2: Returns a Vector2 where each component is clamped between corresponding min and max values.")]
    public class ClampVector2Node : IStaticNode {
        [Input]
        public static Vector2 value = Vector2.zero;

        [Input]
        public static Vector2 min = Vector2.zero;

        [Input]
        public static Vector2 max = Vector2.one;

        [Output]
        public static Vector2 ClampVector2(Vector2 value, Vector2 min, Vector2 max) {
            float x = Mathf.Clamp(value.x, min.x, max.x);
            float y = Mathf.Clamp(value.y, min.y, max.y);
            return new Vector2(x, y);
        }
    }
}