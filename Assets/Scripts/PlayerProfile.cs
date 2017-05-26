using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;

public class PlayerProfile : MonoBehaviour {

	public Canvas player_stat;
	private Text win;
	private Text lose;
    private Text winrate;
	private Text inputText;
	DatabaseReference mDatabaseRef;
	void Awake(){
		#if UNITY_EDITOR
		    FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(
	        "https://game-dev-dabab.firebaseio.com/");
		#else
		    FirebaseApp.DefaultInstance.Options.DatabaseUrl = 
	        new System.Uri("https://game-dev-dabab.firebaseio.com");
		#endif
	}
	void Start(){
		win = player_stat.transform.Find("Win").GetComponent<Text>();
		lose = player_stat.transform.Find("Lose").GetComponent<Text>();
		winrate = player_stat.transform.Find("Winrate").GetComponent<Text>();
		inputText = player_stat.transform.Find("InputName").transform.Find("Text").GetComponent<Text>();

		// Set up the Editor before calling into the realtime database.
		//FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://game-dev-dabab.firebaseio.com");

	}
	public void Search(){
		String name = inputText.text;
		FirebaseDatabase.DefaultInstance
		.GetReference("Player").Child(name)
		.GetValueAsync().ContinueWith(task => {
			if (task.IsFaulted) {
				Debug.Log("Error");
			}
			else if (task.IsCompleted) {
				DataSnapshot snapshot = task.Result;
				if(snapshot.Value != null){
					var stat = snapshot.Value as Dictionary<string, object>;
					int temp_win = 0;
					int temp_lose = 0;
					foreach (var item in stat)
					{
						Debug.Log(item.Key + ": " + item.Value);
						if(item.Key == "win"){
							win.text = "Win : " + item.Value.ToString();
							temp_win = int.Parse(item.Value.ToString());
						}
						else if(item.Key == "lose"){
							lose.text = "Lose : " + item.Value.ToString();
							temp_lose = int.Parse(item.Value.ToString());
						}
					}
					winrate.text =  "Winrate : " + (temp_win / (temp_win + temp_lose * 1.0)).ToString();
				}
			}
		});
	}
}
