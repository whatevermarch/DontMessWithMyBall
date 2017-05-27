using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour {
	private static StatManager instance ;

	public static int kamikaze_red;
	public static int kill_red;
	public static int kamikaze_blue;
	public static int kill_blue;
	public static int team_lose;
	[HideInInspector]
	public static bool gameOver;

	void Awake()
	{
		if(!instance)
			instance = this ;
		else
			Destroy(this.gameObject) ;
		DontDestroyOnLoad(this.gameObject) ;
	}

	void Update()
	{
	//	if (Input.GetKeyUp(KeyCode.Space)) {
	//		Debug.Log ("Kamikaze Red: " + kamikaze_red);
	//		Debug.Log ("Kamikaze Blue: " + kamikaze_blue);
	//		Debug.Log ("Kill Red: " + kill_red);
	//		Debug.Log ("Kill Blue: " + kill_blue);
	//		Debug.Log ("Team Lose: " + team_lose);
	//	}
	}

}
