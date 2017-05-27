using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;
public class TeamMoneyDisplay : NetworkBehaviour {


	public int startMoney;
	[SyncVar][HideInInspector]
	public int teamBlueMoney;
	[SyncVar][HideInInspector]
	public int teamRedMoney;
	private GameObject player;
	private int playerTeam;
	private Text moneyRedText;
	private Text moneyBlueText;

	void Start () {
		player = GameObject.Find("Player(Clone)");
		playerTeam = player.GetComponent<MyPlayerController>().teamNumber;

		teamBlueMoney = startMoney;
		teamRedMoney = startMoney;
		moneyRedText = transform.FindChild("Red").GetComponent<Text>();
		moneyBlueText = transform.FindChild("Blue").GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
		RpcDisplayMoney();
	}

	[ClientRpc]
	void RpcDisplayMoney(){
		moneyRedText.text = teamRedMoney.ToString();
		moneyBlueText.text = teamBlueMoney.ToString();
	}
}
