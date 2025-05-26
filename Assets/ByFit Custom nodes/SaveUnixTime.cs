#pragma warning disable
using UnityEngine;
using System;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes", "SaveUnixTime", tooltip = "Saves the current Unix time to PlayerPrefs\nСохраняет текущее Unix-время в PlayerPrefs", icon = typeof(SaveUnixTime), hasFlowInput = true)]
	[TypeIcons.IconGuid("4d4a08d1e4dd0814e9275217a402a6de")]
	public class SaveUnixTime : IFlowNode {
		[Output] public long UnixTime;

		public void Execute(object graph) {
			long unixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			PlayerPrefs.SetString("SavedUnixTime", unixTime.ToString());
			PlayerPrefs.Save();
			UnixTime = unixTime;

			//Debug.Log("Время сохранения (Unix): " + unixTime);
			//Debug.Log("Дата и время сохранения: " + DateTimeOffset.FromUnixTimeSeconds(unixTime).ToLocalTime());
		}
	}
}
