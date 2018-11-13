using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
	private GlobalEnemyManager _enemyManager;
	private AudioSource _normalSource, _combatSource;
	private bool _combat;
	// Use this for initialization
	void Start () {
		var sources = GetComponents<AudioSource>();
		_normalSource = sources[0];
		_combatSource = sources[1];
		_enemyManager = GameObject.FindObjectOfType<GlobalEnemyManager>();
		_normalSource.Play();
	}

	// Update is called once per frame
	void Update () {
		if (_enemyManager.IsInCombat() && !_combat) {
			_combat = true;
			_combatSource.Play();
		} else if (!_enemyManager.IsInCombat() && _combat) {
			_combat = false;
			_combatSource.Stop();
		}
	}
}
