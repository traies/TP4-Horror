using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {
	public int BulletCount;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PickUp(PickUpManager pickUpManager) {
		pickUpManager.GetComponent<WeaponManager>().AddBullets(BulletCount);
		
		Destroy(gameObject);
	}
}
