using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour {
	private PlayerManager _playerManager;
	private Animator _animator;
	public float HealthPoints;
	private float _hp;
	// Use this for initialization
	void Start () {
		_hp = HealthPoints;
		_animator = GetComponent<Animator>();
		_playerManager = GetComponent<PlayerManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void DeathAnimation()
	{
		_animator.SetTrigger("Dead");
		_playerManager.Die();
		_animator.SetLayerWeight(1, 0);
	}

	public void TakeDamage(float damage) {
		_hp -= damage;
		if (_hp <= 0) {
			DeathAnimation();
		}
	}
}
