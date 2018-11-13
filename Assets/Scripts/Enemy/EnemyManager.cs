﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyManager : MonoBehaviour {
	private NavMeshAgent _agent;
	private Animator _animator;
	private EnemyState _state;

	public float AwarenessRaidus;
	public float AttackRadius;
	public float HitRadius;
	public float AnimationSpeedRandomizationMin, AnimationSpeedRandomizationMax;
	public PlayerHealthManager _player;
	private Collider _hitCollider;

	private AudioSource _HitSound;
	private bool _hitOnAnimation;
	public float HitDamage;

	private HealthManager _healthManager;
	private ZombieSoundManager _zombieSounds;
	private Rigidbody _rb;
	public List<Collider> Limbs;
	private LayerMask _mask;
	private Collider _playerCollision;
	// Use this for initialization
	void Start () {
		_agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_state = EnemyState.IDLE;
		_hitCollider = GetComponent<Collider>();
		_HitSound = GetComponent<AudioSource>();
		_healthManager = GetComponent<HealthManager>();
		_zombieSounds = GetComponent<ZombieSoundManager>();
		_rb = GetComponent<Rigidbody>();
		_player = GameObject.FindObjectOfType<PlayerHealthManager>();
		_playerCollision = GetComponent<Collider>();
		_animator.speed *= Random.value * (AnimationSpeedRandomizationMax - AnimationSpeedRandomizationMin) + AnimationSpeedRandomizationMin;
		_mask = LayerMask.GetMask("Player");
	}

	public enum EnemyState {
		IDLE,
		FOLLOWING,
		ATTACKING,
		DEAD
	}

	// Update is called once per frame
	void Update ()
	{
		var distanceToPlayer = (transform.position - _player.transform.position).magnitude;
		switch (_state) {
			case EnemyState.IDLE: {
				if (distanceToPlayer < AwarenessRaidus) {
					_agent.destination = 	_player.transform.position;
					_state = EnemyState.FOLLOWING;
				}
				break;
			}
			case EnemyState.FOLLOWING: {

				Collider aux;
				if (CheckPlayerProximity(out aux)) {
					transform.LookAt(_player.transform);
					_animator.SetTrigger("Attack");
					_state = EnemyState.ATTACKING;
					_hitOnAnimation = true;
					_agent.isStopped = true;
				} else {
					if (distanceToPlayer < AwarenessRaidus) {
						_agent.destination = _player.transform.position;
					} else {
						_state = EnemyState.IDLE;
					}
					_animator.SetFloat("Walk",1);
				}
				break;
			}
			case EnemyState.ATTACKING: {
				var animationState = _animator.GetCurrentAnimatorStateInfo(0);

				if (animationState.IsName("Attack")) {
					if (animationState.normalizedTime >= 0.4 && animationState.normalizedTime <= 0.6  && _hitOnAnimation) {
						if (CheckPlayerHit()) {
							Hit();
							Debug.Log("Hitting");
							_hitOnAnimation = false;
						}
					}
					if (animationState.normalizedTime >= 1) {
						_animator.SetTrigger("ResetFollowing");
						_state = EnemyState.FOLLOWING;
						_agent.isStopped = false;
						_hitOnAnimation = false;
					}
				}
				break;
			}
			case EnemyState.DEAD: {
				break;
			}
		}
	}



	private bool CheckPlayerProximity(out Collider collider)
	{
		var position = transform.position;
		position.y += 1;
		RaycastHit hit;
		if (Physics.Raycast(position, transform.forward, out hit, AttackRadius, _mask)) {
			if ( hit.collider.tag == "Player") {
				collider = hit.collider;
				return true;
			}
		}
		collider = null;
		return false;
	}

	private bool CheckPlayerHit()
	{
		var position = transform.position;
		position.y += 1;
		RaycastHit hit;
		if (Physics.Raycast(position, transform.forward, out hit, HitRadius, _mask)) {
			if (hit.collider.tag == "Player") {
				return true;
			}
		}
		return false;
	}

	public void Die() {
		_state = EnemyState.DEAD;
		_healthManager.enabled = false;
		_zombieSounds.enabled = false;
		_agent.enabled = false;
		_playerCollision.enabled = false;
		foreach(var collider in Limbs) {
			collider.enabled = false;
		}
	}

	public void Hit() {
		_player.TakeDamage(HitDamage);
		_HitSound.Play();
	}

	public EnemyState GetState()
	{
		return _state;
	}

	void OnAnimatorMove ()
    {
        // Update position based on animation movement using navigation surface height
        Vector3 position = _animator.rootPosition;
        position.y = _agent.nextPosition.y;
        transform.position = position;
    }
}
