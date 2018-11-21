using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsTransformPosition : MonoBehaviour {
	public Transform HandTransform;
	// Use this for initialization
	void Start () {
		transform.SetParent(HandTransform);
	}	
	
	// Update is called once per frame
	void Update () {
	}
}
