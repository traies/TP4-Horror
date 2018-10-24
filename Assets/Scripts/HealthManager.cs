using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	public float InitialHP;
	private Animator _animator;
	private float _hp;
	// Use this for initialization
	void Start () {
		_hp = InitialHP;
		_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void DeathAnimation()
	{
		_animator.SetTrigger("Dead");
	}

	public void TakeDamage(float damage) {
		_hp -= damage;
		if (_hp <= 0) {
			DeathAnimation();
		}
	}
}
