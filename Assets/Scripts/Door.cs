using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	private HingeJoint _hinge;
	private bool _opened;
	private AudioSource _openDoorSound;
	public Transform AnchorPoint;
	public float OpeningFrames, PlayerDistance;
	private float _angleIncrement, _angle;
	private bool _blocked;
	private Collider _collider;
	private LayerMask _mask;
	// Use this for initialization
	void Start () {
			_hinge = GetComponent<HingeJoint>();
			_openDoorSound = GetComponent<AudioSource>();
			_collider = GetComponent<Collider>();
			_opened = false;
			_angleIncrement = 90 / OpeningFrames;
			_mask = LayerMask.GetMask("Player");
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

	void UpdateDoor() {
		if (_opened && _angle > -90) {
			_angle -= _angleIncrement;
			transform.RotateAround(AnchorPoint.position, Vector3.up, -_angleIncrement);
		} else if (!_opened && _angle < 0) {
			_angle += _angleIncrement;
			transform.RotateAround(AnchorPoint.position, Vector3.up, _angleIncrement);
		}
	}
	
	void FixedUpdate() {
		// Raycast player position.
		var dir = _opened? -transform.right :  transform.right;
		Debug.DrawRay(transform.position, dir, Color.red, 1);
		if (!Physics.Raycast(transform.position, dir, PlayerDistance, _mask)) {
			if (_opened && _angle > -90) {
				_angle -= _angleIncrement;
				transform.RotateAround(AnchorPoint.position, Vector3.up, -_angleIncrement);
			} else if (!_opened && _angle < 0) {
				_angle += _angleIncrement;
				transform.RotateAround(AnchorPoint.position, Vector3.up, _angleIncrement);
			}
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

	// void OnCollisionExit(Collision collision) {
	// 	if (collision.collider.tag == "Player") {
	// 		_blocked = false;
	// 	}
	// }
}
