using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbManager : MonoBehaviour {
	public float LimbDamage;
	public HealthManager HealthManager;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage()
	{
		HealthManager.TakeDamage(LimbDamage);
	}
}
