using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class PushFirebase : NetworkBehaviour {

	DatabaseReference mDatabase;

	private void Awake()  
	{
		#if UNITY_EDITOR
			FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
			"https://game-dev-dabab.firebaseio.com/");
		#else
			FirebaseApp.DefaultInstance.Options.DatabaseUrl = 
		new System.Uri("https://game-dev-dabab.firebaseio.com");
		#endif
	}
	void Start() {
		mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
	}

	void Update(){
		if(StatManager.gameOver == true)
			RpcGameOver();
	}
	[ClientRpc]
	void RpcGameOver(){
		StartCoroutine("DelayEndGame", 3f);
	}

	IEnumerator DelayEndGame(float delay)
	{
		yield return new WaitForSeconds(delay);
		EndGame();
		SceneManager.LoadScene ("End Game");
	}
	public void EndGame(){
		if(isServer)
			WriteNewScore (StatManager.kamikaze_red, StatManager.kill_red, StatManager.kamikaze_blue, StatManager.kill_blue, StatManager.team_lose);
	}
	private void WriteNewScore(int kamikaze_red, int kill_red,int kamikaze_blue, int kill_blue, int team_lose) {
		// Create new entry at /user-scores/$userid/$scoreid and at
		// /leaderboard/$scoreid simultaneously
		string key = mDatabase.Child("MatchInfo").Push().Key;
		MatchStat entry = new MatchStat(kamikaze_red, kill_red, kamikaze_blue, kill_blue, team_lose);
		Dictionary<string, System.Object> entryValues = entry.ToDictionary();

		Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
		childUpdates["/matchInfo/" + key] = entryValues;

		mDatabase.UpdateChildrenAsync(childUpdates);
	}

}

public class MatchStat {

	public int kamikaze_red;
	public int kamikaze_blue;
	public int kill_red;
	public int kill_blue;
	public string team_win;

	public MatchStat() {
	}

	public MatchStat(int kamikaze_red, int kill_red,int kamikaze_blue, int kill_blue, int team_lose) {

		this.kamikaze_red = kamikaze_red;
		this.kamikaze_blue = kamikaze_blue;
		this.kill_red = kill_red;
		this.kill_blue = kill_blue;
		if (team_lose == 1) {
			team_win = "Blue";
		} else {
			team_win = "Red";
		}
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["kamikaze_red"] = kamikaze_red;
		result["kamikaze_blue"] = kamikaze_blue;
		result["kill_red"] = kill_red;
		result["kill_blue"] = kill_blue;
		result["team_win"] = team_win;

		return result;
	}
}