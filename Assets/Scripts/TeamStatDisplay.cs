using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TeamStatDisplay : MonoBehaviour {

	// Use this for initialization
	public Text teamWin;
	public Text totalKamikazaRed;
	public Text totalKamikazaBlue;
	public Text totalKillRed;
	public Text totalKillBlue;

	void Start () {
		if (StatManager.team_lose == 1) {
			teamWin.text = "Blue Team Win !!!!";
		} else {
			teamWin.text = "Red Team Win !!!!";
		}
		totalKamikazaRed.text = "Total team Red Kamikaze : " + StatManager.kamikaze_red.ToString();
		totalKamikazaBlue.text = "Total team blue Kamikaza : " + StatManager.kamikaze_blue.ToString();
		totalKillRed.text = "Total team red kill : " + StatManager.kill_red.ToString();
		totalKillBlue.text = "Total team blue kill : " + StatManager.kill_blue.ToString();
	}
}
