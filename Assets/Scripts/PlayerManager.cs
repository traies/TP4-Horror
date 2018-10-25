using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ShootManager))]
[RequireComponent(typeof(AimManager))]
public class PlayerManager : MonoBehaviour {
	Animator _animator;
	ShootManager _shoot;
	private ReloadManager _reload;
	private WeaponManager _weapon;
	private PickUpManager _pickUp;
	AimManager _aimManager;
	PlayerHealthManager _playerHealth;
	public UnityStandardAssets.Characters.FirstPerson.FirstPersonController FirstPersonController;

	// Use this for initialization
	void Start () {
		_aimManager = GetComponent<AimManager>();
		_shoot = GetComponent<ShootManager>();
		_reload = GetComponent<ReloadManager>();
		_weapon = GetComponent<WeaponManager>();
		_pickUp = GetComponent<PickUpManager>();
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
		_aimManager.enabled = false;
		_shoot.enabled = false;
		_reload.enabled = false;
		_weapon.enabled = false;
		_pickUp.enabled = false;
		FirstPersonController.enabled = false;
	}
}
