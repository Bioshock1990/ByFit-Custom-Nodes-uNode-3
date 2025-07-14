using System;
using System.Collections.Generic;
using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Time", "GetUnixTime", tooltip = "Returns current Unix time in seconds.\nВозвращает текущее Unix-время в секундах.", icon = typeof(GetUnixTime), outputs = new Type[] { typeof(long) })]
[TypeIcons.IconGuid("95359488c267cae47aea4481fb0c12e7")]
	public class GetUnixTime : IStaticNode {	
		[Output]
		public static long UnixTime() {
			return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}
	}
}
