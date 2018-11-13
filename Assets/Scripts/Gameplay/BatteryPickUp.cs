using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickUp : PickUpItem {

	public override void PickUp(PickUpManager manager) {
		manager.GetComponent<FlashlightManager>().AddBattery();
		Destroy(gameObject);
	}
}
