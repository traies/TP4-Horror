using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyManager : MonoBehaviour {
	private NavMeshAgent _agent;
	private Animator _animator;
	public Transform Player;
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
	// Use this for initialization
	void Start () {
		_agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_state = EnemyState.IDLE;
		_hitCollider = GetComponent<Collider>();
		_HitSound = GetComponent<AudioSource>();
		_healthManager = GetComponent<HealthManager>();
		_zombieSounds = GetComponent<ZombieSoundManager>();
		_animator.speed *= Random.value * (AnimationSpeedRandomizationMax - AnimationSpeedRandomizationMin) + AnimationSpeedRandomizationMin;
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
		var distanceToPlayer = (transform.position - Player.position).magnitude;
		switch (_state) {
			case EnemyState.IDLE: {
				if (distanceToPlayer < AwarenessRaidus) {
					_agent.destination = 	Player.position;
					_state = EnemyState.FOLLOWING;
				}
				break;
			}
			case EnemyState.FOLLOWING: {

				Collider aux;
				if (CheckPlayerProximity(out aux)) {
					_animator.SetTrigger("Attack");
					Debug.Log("Attack");
					_state = EnemyState.ATTACKING;
					_agent.isStopped = true;
				}

				if (distanceToPlayer < AwarenessRaidus) {
					_agent.destination = Player.position;
				}
				_animator.SetFloat("Walk",1);
				break;
			}
			case EnemyState.ATTACKING: {
				Debug.Log("Agent: " + _agent.isStopped);
				var animationState = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
				if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animationState >= 1) {
					_state = EnemyState.FOLLOWING;
					_agent.isStopped = false;
					_hitOnAnimation = false;
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
		
		Debug.DrawRay(position, transform.forward, Color.white, HitRadius);
		if (Physics.Raycast(position, transform.forward, out hit, HitRadius)) {
			Debug.Log(hit.collider.tag);
			
			if ( hit.collider.tag == "Player") {

				collider = hit.collider;
				return true;
			}
		}
		collider = null;
		return false;
	}

	public void Die() {
		_state = EnemyState.DEAD;
		_healthManager.enabled = false;
		_zombieSounds.enabled = false;
		_agent.enabled = false;
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
