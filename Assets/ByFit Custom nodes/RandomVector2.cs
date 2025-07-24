using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Math/Random", "Random Vector2", 
		tooltip = "Returns a Vector2 with each component randomly between corresponding min and max Vector2 components.", 
		icon = typeof(UnityEngine.Random))]
	[Description("RandomVector2: Generates a Vector2 where x and y are random floats between min.x and max.x, min.y and max.y respectively.")]
	public class RandomVector2Node : IStaticNode {
		[Input]
		public static Vector2 min = Vector2.zero;

		[Input]
		public static Vector2 max = Vector2.one;

		[Output]
		public static Vector2 RandomVector2(Vector2 min, Vector2 max) {
			float xMin = Mathf.Min(min.x, max.x);
			float xMax = Mathf.Max(min.x, max.x);
			float yMin = Mathf.Min(min.y, max.y);
			float yMax = Mathf.Max(min.y, max.y);

			return new Vector2(
				UnityEngine.Random.Range(xMin, xMax),
				UnityEngine.Random.Range(yMin, yMax)
			);
		}
	}
}
