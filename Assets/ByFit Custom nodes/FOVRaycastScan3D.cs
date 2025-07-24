#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/GamePlay", "FOV Raycast Scan 3D", 
	tooltip = "Casts a set of rays from the origin object within horizontal and vertical fields of view.\n" +
	"Inputs:\n" +
	"- origin: the source GameObject from which rays are cast.\n" +
	"- horizontalFOV: horizontal field of view angle in degrees.\n" +
	"- horizontalRayCount: number of rays cast horizontally.\n" +
	"- verticalFOV: vertical field of view angle in degrees.\n" +
	"- verticalRayCount: number of rays cast vertically.\n" +
	"- rayDistance: maximum ray length.\n" +
	"- raycastOffset: offset vector from origin position for ray start points.\n" +
	"- offsetRotationEuler: additional rotation offset in degrees applied to ray directions.\n" +
	"- layerMask: layer mask for raycast collision filtering.\n" +
	"- targetTags: list of tags to filter hit objects.\n" +
	"- debugDraw: toggle for ray debug visualization.\n" +
	"Outputs:\n" +
	"- hitObjects: sorted list of GameObjects hit by rays (nearest first).\n" +
	"- closestDistance: distance to the closest hit object.\n" +
	"- hitInfo: RaycastHit info of the closest hit.\n" +
	"You can use horizontal FOV only, vertical FOV only, or both simultaneously.\n" +
	"Setting the ray count to 0 for either direction disables rays in that direction.",
	icon = typeof(FOVRaycastScan3D))]
[TypeIcons.IconGuid("f08d59e8e8db9034eac629a01fc655a2")]
public class FOVRaycastScan3D : IFlowNode {
	[Input] public GameObject origin;
	[Input] public float horizontalFOV = 90f;
	[Input] public int horizontalRayCount = 10;
	[Input] public float verticalFOV = 60f;
	[Input] public int verticalRayCount = 5;
	[Input] public float rayDistance = 10f;
	[Input] public Vector3 raycastOffset = Vector3.zero;
	[Input] public Vector3 offsetRotationEuler = Vector3.zero;
	[Input] public LayerMask layerMask = ~0;
	[Input] public List<string> targetTags;
	[Input] public bool debugDraw = true;

	[Output] public List<GameObject> hitObjects;
	[Output] public float closestDistance;
	[Output] public RaycastHit hitInfo;

	private struct HitRecord {
		public GameObject obj;
		public float dist;
		public RaycastHit hit;
	}

	public void Execute(object graph) {
		hitObjects = new List<GameObject>();
		closestDistance = float.MaxValue;
		hitInfo = new RaycastHit();

		if(origin == null)
			return;

		List<HitRecord> records = new List<HitRecord>();

		Transform t = origin.transform;
		Vector3 originPos = t.position + raycastOffset;

		Quaternion baseRotation = Quaternion.LookRotation(t.forward, t.up);
		Quaternion offsetRotation = Quaternion.Euler(offsetRotationEuler);
		Quaternion finalRotation = baseRotation * offsetRotation;

		bool hasHorizontal = horizontalRayCount > 0;
		bool hasVertical = verticalRayCount > 0;

		int vCount = Mathf.Max(verticalRayCount, 1);
		int hCount = Mathf.Max(horizontalRayCount, 1);
		float hStep = horizontalFOV / Mathf.Max(horizontalRayCount - 1, 1);
		float vStep = verticalFOV / Mathf.Max(verticalRayCount - 1, 1);
		float hStart = -horizontalFOV / 2f;
		float vStart = -verticalFOV / 2f;

		for(int v = 0; v < (hasVertical ? verticalRayCount : 1); v++) {
			float vAngle = hasVertical ? vStart + v * vStep : 0f;

			for(int h = 0; h < (hasHorizontal ? horizontalRayCount : 1); h++) {
				float hAngle = hasHorizontal ? hStart + h * hStep : 0f;

				Quaternion rayRot = finalRotation * Quaternion.Euler(vAngle, hAngle, 0);
				Vector3 dir = rayRot * Vector3.forward;

				Ray ray = new Ray(originPos, dir);
				if(Physics.Raycast(ray, out RaycastHit hit, rayDistance, layerMask)) {
					if(targetTags == null || targetTags.Count == 0 || targetTags.Contains(hit.collider.tag)) {
						GameObject obj = hit.collider.gameObject;
						records.Add(new HitRecord {
							obj = obj,
							dist = hit.distance,
							hit = hit
						});
						if(debugDraw)
							Debug.DrawRay(originPos, dir * hit.distance, Color.red, 0.1f);
					} else {
						if(debugDraw)
							Debug.DrawRay(originPos, dir * rayDistance, Color.green, 0.1f);
					}
				} else {
					if(debugDraw)
						Debug.DrawRay(originPos, dir * rayDistance, Color.green, 0.1f);
				}
			}
		}

		records.Sort((a, b) => a.dist.CompareTo(b.dist));
		foreach(var rec in records) {
			if(!hitObjects.Contains(rec.obj)) {
				hitObjects.Add(rec.obj);
			}
		}
		if(records.Count > 0) {
			closestDistance = records[0].dist;
			hitInfo = records[0].hit;
		}
	}
}
