using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimManager : MonoBehaviour {
	public Camera GunCamera;
	public Camera MainCamera;
	public Canvas Crosshairs;
	public float AimingFovMultiplier;
	public float FrameIncrement;
	private float _gunCameraMinFov, _gunCameraMaxFov;
	private float _mainCameraMinFov, _mainCameraMaxFov;
	[SerializeField] private float _increment;
	private bool _aiming;
	private ReloadManager _reloadManager;
	public float HeadBobSpeed, HeadBobAmplitude;
	private ShootManager _shootManager;
	private PlayerManager _playerManager;
	private PlayerHealthManager _playerHealth;
	// Use this for initialization
	void Start () {
		_gunCameraMaxFov = GunCamera.fieldOfView;
		_mainCameraMaxFov = MainCamera.fieldOfView;
		_gunCameraMinFov = _gunCameraMaxFov * AimingFovMultiplier;
		_mainCameraMinFov = _mainCameraMaxFov * AimingFovMultiplier;
		_reloadManager = GetComponent<ReloadManager>();
		_shootManager = GetComponent<ShootManager>();
		_playerManager = _shootManager.PlayerManager;
		_playerHealth = _playerManager.GetComponent<PlayerHealthManager>();
	}

	private bool CheckIfAimingInput()
	{
		return Input.GetButton("Aim") || Input.GetButton("Shoot");
	}
	// Update is called once per frame
	void Update () {
		if (_reloadManager.IsReloading()) {
			_increment = +FrameIncrement;
			Crosshairs.enabled = false;
			_aiming = false;
		} else {
			if (!_aiming) {
				if (CheckIfAimingInput()) {
					// Start aiming
					_increment = -FrameIncrement;
					Crosshairs.enabled = true;
					_aiming = true;
				}
			} else {
				if (!(CheckIfAimingInput())) {
					// End aiming
					_increment = +FrameIncrement;
					Crosshairs.enabled = false;
					_aiming = false;
				}
			}
		}

		var gFov = Mathf.Clamp(GunCamera.fieldOfView + _increment, _gunCameraMinFov, _gunCameraMaxFov);
		var mFov = Mathf.Clamp(MainCamera.fieldOfView + _increment, _mainCameraMinFov, _mainCameraMaxFov);
		if (gFov != GunCamera.fieldOfView) {
			GunCamera.fieldOfView = gFov;
		}
		if (mFov != MainCamera.fieldOfView) {
			MainCamera.fieldOfView = mFov;
		}
	}

	public void UpdateAim()
	{
		if (_aiming) {
			float xBob = Mathf.Sin(Time.realtimeSinceStartup * HeadBobSpeed );
			float yBob = Mathf.Cos(Time.realtimeSinceStartup * HeadBobSpeed);
			float healthMultiplier = 1 - _playerHealth.GetHealthNormalized();
			MainCamera.transform.Rotate(xBob * HeadBobAmplitude * healthMultiplier, yBob * HeadBobAmplitude * healthMultiplier, 0);
		}
		_playerManager.IsAiming(_aiming);
	}

	public bool IsAiming() {
		return _aiming;
	}

	public void ResetAiming()
	{
		GunCamera.fieldOfView = _gunCameraMaxFov;
		MainCamera.fieldOfView = _mainCameraMaxFov;
		_increment = 0;
		Crosshairs.enabled = false;
		_aiming = false;
		_playerManager.IsAiming(false);
	}
}
