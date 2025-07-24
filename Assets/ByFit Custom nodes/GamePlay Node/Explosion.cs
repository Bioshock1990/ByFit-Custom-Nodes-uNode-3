#pragma warning disable
using UnityEngine;
using System.Collections.Generic;
using MaxyGames.UNode;

[NodeMenu("ByFit Custom nodes/GamePlay", "Explosion", tooltip = "Applies explosion force to objects with the specified tag within the given radius.", icon = typeof(Explosion))]
[TypeIcons.IconGuid("885cf0547da7b9845ba673307eb90870")]
public class Explosion : IFlowNode {	
	[Input]
	public float explosionImpulse = 700F;
	[Input]
	public Vector3 explosionPosition;
	[Input]
	public float explosionRadius = 10F;
	[Input]
	public float upwardsModifier = 10F;
	[Input]
	public string tag = "Explosion Objects";
	
	public void Execute(object graph) {
		tag = "Explosion";
		var Colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);
		foreach(Collider loopValue in Colliders) {
			var Node_11 = loopValue;
			if(Node_11.gameObject.CompareTag(tag)) {
				Node_11.GetComponent<Rigidbody>().AddExplosionForce(explosionImpulse, explosionPosition, explosionRadius, upwardsModifier, ForceMode.Force);
			}
		}
	}
}

