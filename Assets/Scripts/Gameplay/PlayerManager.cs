using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
	Animator _animator;

	private PickUpManager _pickUp;
	PlayerHealthManager _playerHealth;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FirstPersonController;
	private bool _isAiming;
	// Use this for initialization
	void Start () {
		_pickUp = GetComponent<PickUpManager>();
		FirstPersonController = GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_isAiming) {
			FirstPersonController.StopMovement = true;
		} else {
			FirstPersonController.StopMovement = false;
		}
	}

	public void IsAiming(bool isAiming) {
		_isAiming = isAiming;
	}

	public void Die() {
		_pickUp.enabled = false;
		FirstPersonController.enabled = false;
	}
}
