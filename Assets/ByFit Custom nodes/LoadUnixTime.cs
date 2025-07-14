#pragma warning disable
using UnityEngine;
using System;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes/Time", "LoadUnixTime", tooltip = "Loads Unix time from PlayerPrefs and calculates elapsed seconds\nЗагружает Unix-время из PlayerPrefs и считает, сколько секунд прошло", icon = typeof(LoadUnixTime), hasFlowInput = true)]
	[TypeIcons.IconGuid("b6d14458f79096e4ebf4631134526c4b")]
	public class LoadUnixTime : IFlowNode {
		[Output] public long UnixTime;
		[Output] public int ElapsedSeconds;

		[Input(primary = true)]

		public void Execute(object graph) {
			string savedTimeStr = PlayerPrefs.GetString("SavedUnixTime", "0");
			long savedUnixTime = long.TryParse(savedTimeStr, out var parsedTime) ? parsedTime : 0;
			long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			UnixTime = now;
			ElapsedSeconds = (int)(now - savedUnixTime);

			//Debug.Log("Текущее Unix-время: " + now);
			//Debug.Log("Прошло секунд с момента сохранения: " + ElapsedSeconds);
		}
	}
}
