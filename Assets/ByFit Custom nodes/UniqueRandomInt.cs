using System;
using System.Collections.Generic;
using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Math/Random", "Unique Random Int", tooltip = "Returns a unique random int from range. Resets when exhausted or manually.", icon = typeof(UnityEngine.Random))]
	[TypeIcons.IconGuid("95359488c267cae47aea4481fb0c12e7")]
	public class UniqueRandomInt : IStaticNode {
		private static List<int> pool = new List<int>();
		private static int lastMin;
		private static int lastMax;

		[Input]
		public static int min;

		[Input]
		public static int max;

		[Input]
		public static bool resetNow = false;

		[Output]
		public static int UniqueRandomIntMethod(int min, int max, bool resetNow) {
			if (min > max) {
				var temp = min;
				min = max;
				max = temp;
			}

			if (resetNow || pool.Count == 0 || min != lastMin || max != lastMax) {
				lastMin = min;
				lastMax = max;
				ResetPool(min, max);
			}

			int index = UnityEngine.Random.Range(0, pool.Count);
			int result = pool[index];
			pool.RemoveAt(index);

			if (pool.Count == 0) {
				ResetPool(min, max);
			}

			return result;
		}

		private static void ResetPool(int min, int max) {
			pool.Clear();
			for (int i = min; i <= max; i++) {
				pool.Add(i);
			}
		}
	}
}
