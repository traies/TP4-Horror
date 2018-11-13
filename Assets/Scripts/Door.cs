using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private HingeJoint _hinge;
	private bool _opened;
	private AudioSource _openDoorSound;
	public Transform AnchorPoint;
	public float OpeningFrames;
	private float _angleIncrement, _angle;
	// Use this for initialization
	void Start () {
			_hinge = GetComponent<HingeJoint>();
			_openDoorSound = GetComponent<AudioSource>();
			_opened = false;
			_angleIncrement = 90 / OpeningFrames;
	}

	public void Interact () {
		_openDoorSound.Play();
		if (!_opened) {
			_opened = true;
			// var hinge = _hinge.spring;
			// hinge.targetPosition = -90;
			// _hinge.spring = hinge;
			// _opened = true;
		} else {
			_opened = false;
			// var hinge = _hinge.spring;
			// hinge.targetPosition = 0;
			// _hinge.spring = hinge;
			// _opened = false;
		}
	}

	void Update() {
		if (_opened && _angle > -90) {
			_angle -= _angleIncrement;
			transform.RotateAround(AnchorPoint.position, Vector3.up, -_angleIncrement);
		} else if (!_opened && _angle < 0) {
			_angle += _angleIncrement;
			transform.RotateAround(AnchorPoint.position, Vector3.up, _angleIncrement);
		}
	}

	public string Prompt() {
		return _opened ? "Close" : "Open";
	}



	IEnumerator Open() {
		for (int i = 0; i < OpeningFrames; i++) {

			yield return null;
		}
	}


}
