﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerName : MonoBehaviour {

	// Use this for initialization
	private Quaternion OrgRotation;
	private Vector3 OrgPosition;
	void Start () {
		OrgRotation = transform.rotation;
    	OrgPosition = transform.parent.transform.position - transform.position;
		GetComponent<TextMesh>().text = gameObject.GetComponentInParent<MyPlayerController>().playerName;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.rotation = OrgRotation;
		transform.position = transform.parent.position - OrgPosition;
		
	}
}
