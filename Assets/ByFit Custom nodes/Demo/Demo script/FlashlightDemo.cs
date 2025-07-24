#pragma warning disable
using UnityEngine;
using System.Collections.Generic;

public class FlashlightDemo : MonoBehaviour {	
	private ByFit.Flashlight Flashlight_2 = new ByFit.Flashlight() { Light = null };
	public KeyCode Key = KeyCode.F;
	public bool EnableFlicker;
	public float FlickerSpeed = 0.1F;
	
	private void Update() {
		Flashlight_2.Light = this.GetComponent<Light>();
		Flashlight_2.Key = Key;
		Flashlight_2.EnableFlicker = EnableFlicker;
		Flashlight_2.FlickerSpeed = FlickerSpeed;
		Flashlight_2.Execute(this);
	}
	
	public void Enable_Flicker() {
		EnableFlicker = !(EnableFlicker);
	}
}

