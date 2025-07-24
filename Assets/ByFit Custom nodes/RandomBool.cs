using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Math/Random", "Random Bool (0-100%)", tooltip = "Returns true with a chance (0-100).", icon = typeof(UnityEngine.Random))]
	public class RandomBoolNode : IStaticNode {
		[Input]
		public static float chancePercent = 50f;

		[Output]
		public static bool RandomBool(float chancePercent) {
			float clamped = Mathf.Clamp(chancePercent, 0f, 100f);
			float roll = UnityEngine.Random.Range(0f, 100f);
			return roll < clamped;
		}
	}
}
