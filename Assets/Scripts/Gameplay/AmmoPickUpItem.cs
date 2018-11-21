using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType{
	PISTOL,
	SHOTGUN,
}

public class AmmoPickUpItem : PickUpItem {

	public int BulletCount;
	public WeaponType Type;

	public override void PickUp(PickUpManager pickUpManager) {
		switch(Type) {
			case WeaponType.PISTOL:
				pickUpManager.PistolManager.AddBullets(BulletCount);
				break;
			case WeaponType.SHOTGUN:
				pickUpManager.ShotgunManager.AddBullets(BulletCount);
				break;
		}
		Destroy(gameObject);
	}
}
