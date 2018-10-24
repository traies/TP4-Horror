using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RunManager))]
[RequireComponent(typeof(ShootManager))]
public class PlayerManager : MonoBehaviour {
	Animator _animator;
	RunManager _run;
	ShootManager _shoot;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Die() {
		
	}
}
