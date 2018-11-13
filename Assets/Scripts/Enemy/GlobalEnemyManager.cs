using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnemyManager : MonoBehaviour {
	private int _awareEnemies;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void AddAware() {
		_awareEnemies++;
	}

	public void RemoveAware() {
		_awareEnemies--;
	}

	public bool IsInCombat() {
		return _awareEnemies > 0;
	}
}
