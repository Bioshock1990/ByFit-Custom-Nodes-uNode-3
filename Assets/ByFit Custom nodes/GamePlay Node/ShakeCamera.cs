#pragma warning disable
using UnityEngine;
using MaxyGames.UNode;

namespace ByFit {
	[NodeMenu("ByFit Custom nodes", "ShakeCamera", tooltip = "Shakes the camera with given intensity and duration\nТрясёт камеру с заданной силой и длительностью", icon = typeof(ShakeCamera), hasFlowInput = true)]
	[TypeIcons.IconGuid("6925306a35c55e24ebfa2805dc914aff")]
	public class ShakeCamera : IFlowNode {
		[Hide][Input] public MonoBehaviour Caller;     // Объект для запуска корутины
		[Input] public Camera TargetCamera;      // Камера
		[Input] public float Intensity = 0.3f;   // Сила тряски
		[Input] public float Duration = 0.5f;    // Длительность

		private float shakeTimer;

		public void Execute(object graph) {
			if (TargetCamera == null) {
				TargetCamera = Camera.main;
			}
			if (Caller != null && TargetCamera != null) {
				// Сохраняем позицию камеры именно перед тряской (для каждой тряски)
				Vector3 originalPosition = TargetCamera.transform.localPosition;
				shakeTimer = Duration;
				Caller.StartCoroutine(Shake(originalPosition));
			}
		}

		private System.Collections.IEnumerator Shake(Vector3 originalPosition) {
			while (shakeTimer > 0) {
				shakeTimer -= Time.deltaTime;
				TargetCamera.transform.localPosition = originalPosition + Random.insideUnitSphere * Intensity;
				yield return null;
			}
			TargetCamera.transform.localPosition = originalPosition;
		}
	}
}
