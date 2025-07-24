#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class FovRaycastDemo : MonoBehaviour {	
	private FOVRaycastScan3D FOV_19 = new FOVRaycastScan3D() { origin = null, targetTags = null, hitObjects = null };
	public List<GameObject> hitObjects = new List<GameObject>();
	public float maxDistance;
	public float horizontalFOV;
	public int horizontalRayCount;
	public float verticalFOV;
	public int verticalRayCount;
	public Vector3 raycastOffset;
	public Vector3 offsetRotationEuler;
	public LayerMask layerMask = ((LayerMask)(-1));
	public bool debugDraw;
	public List<string> targetTags = new List<string>();
	
	private void Update() {
		FOV_19.origin = this.gameObject;
		FOV_19.horizontalFOV = horizontalFOV;
		FOV_19.horizontalRayCount = horizontalRayCount;
		FOV_19.verticalFOV = verticalFOV;
		FOV_19.verticalRayCount = verticalRayCount;
		FOV_19.rayDistance = maxDistance;
		FOV_19.raycastOffset = raycastOffset;
		FOV_19.offsetRotationEuler = offsetRotationEuler;
		FOV_19.layerMask = layerMask;
		FOV_19.targetTags = targetTags;
		FOV_19.debugDraw = debugDraw;
		FOV_19.Execute(this);
		hitObjects = FOV_19.hitObjects;
	}
}

