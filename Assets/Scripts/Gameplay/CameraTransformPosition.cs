using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformPosition : MonoBehaviour {
	public Transform HeadTransform;
	// Use this for initialization
	void Start () {
		transform.SetParent(HeadTransform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
