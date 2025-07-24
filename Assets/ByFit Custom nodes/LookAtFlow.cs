#pragma warning disable
using UnityEngine;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/Transform", "LookAt with Axis Lock", tooltip = "Rotates source transform to look at target transform with optional axis locking.", icon = typeof(Transform))]
public class LookAtFlow : IFlowNode {
	[Input]
	public Transform source;

	[Input]
	public Transform target;

	[Input]
	public bool lockX = false;

	[Input]
	public bool lockY = false;

	[Input]
	public bool lockZ = false;

	public void Execute(object graph) {
		var src = source;
		var tgt = target;
		if(src == null || tgt == null) return;

		Vector3 dir = tgt.position - src.position;
		if(dir.sqrMagnitude < 0.0001f) return;

		Quaternion targetRot = Quaternion.LookRotation(dir.normalized, Vector3.up);
		Vector3 euler = targetRot.eulerAngles;

		if(lockX) euler.x = src.rotation.eulerAngles.x;
		if(lockY) euler.y = src.rotation.eulerAngles.y;
		if(lockZ) euler.z = src.rotation.eulerAngles.z;

		src.rotation = Quaternion.Euler(euler);
	}
}
