using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpManager : MonoBehaviour {
	public Transform WorldCamera;
	public float PickUpDistance;
	public TextMeshProUGUI PickUpPrompt;
	private LayerMask mask;
	public AudioSource PickUpSound;
	// Use this for initialization
	void Start () {
		mask = LayerMask.GetMask("Default");
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		if(Physics.Raycast(WorldCamera.position, WorldCamera.forward, out hit, PickUpDistance, mask) && hit.collider.tag == "ItemPickUp") {
			if (Input.GetButton("PickUp")) {
				PickUpSound.Play();
				hit.collider.GetComponent<PickUpItem>().PickUp(this);
			}
			PickUpPrompt.enabled = true;
		} else {
			PickUpPrompt.enabled = false;
		}
	}
}
