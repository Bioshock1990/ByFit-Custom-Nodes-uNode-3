using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Math/Random", "Random Vector3",
		tooltip = "Returns a Vector3 with each component randomly between corresponding min and max Vector3 components.",
		icon = typeof(UnityEngine.Random))]
	[Description("RandomVector3: Generates a Vector3 where x, y, and z are random floats between min.x/max.x, min.y/max.y, and min.z/max.z respectively.")]
	public class RandomVector3Node : IStaticNode {
		[Input]
		public static Vector3 min = Vector3.zero;

		[Input]
		public static Vector3 max = Vector3.one;

		[Output]
		public static Vector3 RandomVector3(Vector3 min, Vector3 max) {
			float xMin = Mathf.Min(min.x, max.x);
			float xMax = Mathf.Max(min.x, max.x);
			float yMin = Mathf.Min(min.y, max.y);
			float yMax = Mathf.Max(min.y, max.y);
			float zMin = Mathf.Min(min.z, max.z);
			float zMax = Mathf.Max(min.z, max.z);

			return new Vector3(
				UnityEngine.Random.Range(xMin, xMax),
				UnityEngine.Random.Range(yMin, yMax),
				UnityEngine.Random.Range(zMin, zMax)
			);
		}
	}
}
