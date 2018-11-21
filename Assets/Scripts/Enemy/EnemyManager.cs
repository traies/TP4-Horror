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
	private EnemyState _state;

	public float AwarenessRadius, InitialAwarenessRadius;
	public float AttackRadius;
	public float HitRadius;
	public float AnimationSpeedRandomizationMin, AnimationSpeedRandomizationMax;
	private PlayerHealthManager _player;
	private Collider _hitCollider;

	private AudioSource _HitSound;
	private bool _hitOnAnimation;
	public float HitDamage, LookAtDistance, LookAtSmoothRotation;

	private HealthManager _healthManager;
	private ZombieSoundManager _zombieSounds;
	private Rigidbody _rb;
	public List<Collider> Limbs;
	private LayerMask _mask;
	private Collider _playerCollision;
	private GlobalEnemyManager _globalEnemyManager;
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
		_globalEnemyManager = GameObject.FindObjectOfType<GlobalEnemyManager>();
	}

	public enum EnemyState {
		IDLE,
		FOLLOWING,
		ATTACKING,
		DEAD,
		HIT,
	}

	// Update is called once per frame
	void Update ()
	{
		var distanceToPlayer = (transform.position - _player.transform.position).magnitude;

		switch (_state) {
			case EnemyState.IDLE: {
				if (distanceToPlayer < InitialAwarenessRadius) {
					_agent.destination = _player.transform.position;
					_state = EnemyState.FOLLOWING;
					_globalEnemyManager.AddAware();
				} else {
					_animator.SetFloat("Walk", 0);
				}
				break;
			}
			case EnemyState.FOLLOWING: {

				Collider aux;
				if (distanceToPlayer < LookAtDistance) {
					var targetRotation = Quaternion.LookRotation(_player.transform.position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, LookAtSmoothRotation * Time.deltaTime);
				}
				if (CheckPlayerProximity(out aux)) {
					_animator.SetTrigger("Attack");
					_state = EnemyState.ATTACKING;
					_hitOnAnimation = true;
					_agent.isStopped = true;
					_animator.SetFloat("Walk", 0);
				} else {
					if (distanceToPlayer < AwarenessRadius) {
						_agent.destination = _player.transform.position;
					} else {
						_state = EnemyState.IDLE;
						_globalEnemyManager.RemoveAware();
					}
					_animator.SetFloat("Walk", _agent.velocity.magnitude / _agent.speed);
				}
				break;
			}
			case EnemyState.ATTACKING: {
				var animationState = _animator.GetCurrentAnimatorStateInfo(0);

				if (animationState.IsName("Attack")) {
					if (animationState.normalizedTime >= 0.4 && animationState.normalizedTime <= 0.6  && _hitOnAnimation) {
						if (CheckPlayerHit()) {
							Hit();
						}
						_hitOnAnimation = false;
					}
				}
				if (!_hitOnAnimation && animationState.IsName("Following")) {
						_state = EnemyState.FOLLOWING;
						_agent.isStopped = false;
						_hitOnAnimation = false;
					}
				break;
			}
			case EnemyState.HIT: {
				var animationState = _animator.GetCurrentAnimatorStateInfo(0);
				if (animationState.IsName("Following") && !_animator.GetBool("Hit")) {
					_state = EnemyState.FOLLOWING;
					_agent.isStopped = false;
				}
				break;
			}
			case EnemyState.DEAD: {
				break;
			}
		}
		var worldDeltaPosition = _agent.nextPosition - transform.position;
		// Pull agent towards character
		if (worldDeltaPosition.magnitude > _agent.radius) {
			_agent.nextPosition = transform.position - 0.9f*worldDeltaPosition;
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
		if (_state != EnemyState.IDLE) {
			_globalEnemyManager.RemoveAware();
		}
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

	public void EnemyIsHit() {
		if (_state != EnemyState.DEAD && _state != EnemyState.HIT) {
			_state = EnemyState.HIT;
			_animator.SetTrigger("Hit");
			_agent.isStopped = true;
			_animator.SetFloat("Walk", 0);
		}
	}
}
