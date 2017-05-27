using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuildingController : NetworkBehaviour {

	public int buildingHealth;
	public int buildingTeam;

	void Update () {
		if (buildingHealth == 0) {
			Debug.Log("Hello World");		
		}
	}
}
