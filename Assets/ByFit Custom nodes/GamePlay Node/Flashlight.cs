#pragma warning disable
using System;
using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes", "Flashlight", tooltip = "Toggles flashlight and simulates a flickering effect when blinking is enabled", icon = typeof(ByFit.Flashlight), hasFlowInput = true, inputs = new Type[] { typeof(Light), typeof(KeyCode), typeof(bool), typeof(float) }, outputs = new Type[] { typeof(bool) })]
	[TypeIcons.IconGuid("d6c07fd3070759d4db216d88f1ab3843")]
	public class Flashlight : IFlowNode {
		[Input] public Light Light;
		[Input] public KeyCode Key = KeyCode.F;
		[Input] public bool EnableFlicker = false;
		[Input] public float FlickerSpeed = 0.1f;

		private bool isOn = false;
		private float nextFlickerTime = 0;
		private float baseIntensity = 1f;

		public void Execute(object graph) {
			if (Input.GetKeyDown(Key) && Light != null) {
				isOn = !isOn;
				Light.enabled = isOn;

				if (isOn) {
					baseIntensity = Light.intensity + 0.5f;
				}

				nextFlickerTime = Time.time + FlickerSpeed;
			}

			if (EnableFlicker && isOn && Light != null) {
				if (Time.time >= nextFlickerTime) {
					Light.intensity = UnityEngine.Random.Range(0f, baseIntensity);
					Light.enabled = UnityEngine.Random.value > 0.2f;
					nextFlickerTime = Time.time + UnityEngine.Random.Range(FlickerSpeed * 0.5f, FlickerSpeed * 1.5f);
				}
			}
		}
	}
}
