using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUpItem : PickUpItem {

	public int BulletCount;

	public override void PickUp(PickUpManager pickUpManager) {
		pickUpManager.GetComponent<WeaponManager>().AddBullets(BulletCount);
		Destroy(gameObject);
	}
}
