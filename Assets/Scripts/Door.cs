using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private HingeJoint _hinge;
	private bool _opened;
	private AudioSource _openDoorSound;
	public Transform AnchorPoint;
	public float OpeningFrames;
	// Use this for initialization
	void Start () {
			_hinge = GetComponent<HingeJoint>();
			_openDoorSound = GetComponent<AudioSource>();
			_opened = false;
	}

	public void Interact () {
		_openDoorSound.Play();
		if (!_opened) {

			StartCoroutine(Open());
			// var hinge = _hinge.spring;
			// hinge.targetPosition = -90;
			// _hinge.spring = hinge;
			// _opened = true;
		} else {
			// var hinge = _hinge.spring;
			// hinge.targetPosition = 0;
			// _hinge.spring = hinge;
			// _opened = false;
		}
	}

	public string Prompt() {
		return _opened ? "Close" : "Open";
	}

	IEnumerator Open() {
		
		for (int i = 0; i < OpeningFrames; i++) {
			transform.RotateAround(AnchorPoint.position, Vector3.up, -10);
			yield return null;
		}
	}
}
