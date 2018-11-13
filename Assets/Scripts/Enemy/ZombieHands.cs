using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHands : MonoBehaviour {
	PlayerHealthManager _playerHealthManager;
	public EnemyManager EnemyManager;
	// Use this for initialization
	void Start () {
		_playerHealthManager = FindObjectOfType<PlayerHealthManager>();
	}

	void OnTriggerEnter(Collider collider) {
		// if (EnemyManager.GetState() == EnemyManager.EnemyState.ATTACKING && collider.tag == "Player") {
		// 	EnemyManager.Hit();
		// }
	}
}
