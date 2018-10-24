﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AimManager))]
public class ShootManager : MonoBehaviour {
	private Animator _playerAnimator;
	private Transform _rightShoulder;
	private Transform _leftShoulder;
	

	public AudioSource Weapon;
	public Camera Camera;
	public float _timeSinceLastShot;
	public float ShootingTimeout;
	public ParticleSystem MuzzleFlash;

	private ParticlePool _sparklesPool;
	private ParticlePool _bloodPool;
	private AimManager _aimManager;
	// Use this for initialization
	void Start () {
		_playerAnimator = GetComponent<Animator>();
		var particlePools = GetComponents<ParticlePool>();
		_sparklesPool = particlePools[0];
		_bloodPool = particlePools[1];
		_aimManager = GetComponent<AimManager>();
	}
	

	// Update is called once per frame
	void Update () {
		_timeSinceLastShot += Time.deltaTime;

		if (Input.GetButton("Shoot")) {
			if (_timeSinceLastShot >= ShootingTimeout && _aimManager.IsAiming()) {
				Shoot();
			}
		} 
		if (Input.GetButtonUp("Shoot")) {
			_playerAnimator.SetBool("Shooting", false);
		}
	}


	private void Shoot() 
	{
		_timeSinceLastShot = 0;
		_playerAnimator.SetBool("Shooting", true);
		Weapon.Play();
		MuzzleFlash.Play();

		// Draw a raycast and collision effect
		RaycastHit hit;
		if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hit, 1000)) {
			ParticlePool particlePool;
			// Check if another player was hit;
			if (hit.collider.tag == "CharacterCollision") {
				// Make other player take damage
				var limbController = hit.collider.GetComponent<LimbManager>();
				limbController.TakeDamage();
				particlePool = _bloodPool;
			} else {
				particlePool = _sparklesPool;
			}
			var particleSystem = particlePool.GetParticleSystem();
			particleSystem.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(hit.normal));
			particleSystem.Play();
			particlePool.ReleaseParticleSystem(particleSystem);
		}
	}
}