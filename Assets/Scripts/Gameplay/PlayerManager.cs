using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour {
	Animator _animator;
	public Canvas DeathScreen;
	private PickUpManager _pickUp;
	PlayerHealthManager _playerHealth;
	private UnityStandardAssets.Characters.FirstPerson.FirstPersonController FirstPersonController;
	private bool _isAiming, _dead;
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

		if (_dead && Input.anyKeyDown) {
			
			SceneManager.LoadScene(0);
		}
	}

	public void IsAiming(bool isAiming) {
		_isAiming = isAiming;
	}

	public void Die() {
		_dead = true;
		_pickUp.enabled = false;
		FirstPersonController.m_MouseLook.lockCursor = false;
		
		DeathScreen.enabled = true;
	}
}
