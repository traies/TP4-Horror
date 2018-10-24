using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RunManager))]
[RequireComponent(typeof(ShootManager))]
[RequireComponent(typeof(AimManager))]
public class PlayerManager : MonoBehaviour {
	Animator _animator;
	RunManager _run;
	ShootManager _shoot;
	AimManager _aimManager;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController FirstPersonController;

	// Use this for initialization
	void Start () {
		_aimManager = GetComponent<AimManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_aimManager.IsAiming()) {
			FirstPersonController.StopMovement = true;
		} else {
			FirstPersonController.StopMovement = false;
		}
	}

	public void Die() {

	}
}
