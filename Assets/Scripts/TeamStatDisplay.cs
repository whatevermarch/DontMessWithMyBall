using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeamStatDisplay : MonoBehaviour {

	// Use this for initialization
	public Text teamWin;
	void Start () {
		if (StatManager.team_lose == 1) {
			teamWin.text = "Blue Team Win !!!!";
		} else {
			teamWin.text = "Red Team Win !!!!";
		}
	}
}
