using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(RunManager))]
[RequireComponent(typeof(ShootManager))]
public class PlayerManager : MonoBehaviour {
	Animator _animator;
	HealthManager _health;
	RunManager _run;
	ShootManager _shoot;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
