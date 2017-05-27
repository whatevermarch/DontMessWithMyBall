using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class BuildingController : NetworkBehaviour {

	public int buildingHealth;
	public int buildingTeam;
	public int teamLose = -1;
	[HideInInspector]
	public bool gameOver;

	void Start () {
		gameOver = false;
	}
	void Update () {
		if (buildingHealth == 0) {
			gameOver = true;
			StatManager.gameOver = true;
			teamLose = buildingTeam;
			StatManager.team_lose = buildingTeam;
			RpcGameOver ();
		}
	}

	[ClientRpc]
	void RpcGameOver(){
		StartCoroutine("DelayEndGame", 3f);
	}

	IEnumerator DelayEndGame(float delay)
	{
		yield return new WaitForSeconds(delay);
		SceneManager.LoadScene ("End Game");
	}
}
