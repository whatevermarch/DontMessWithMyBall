using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkManager))]
public class NetworkManagerForTest : MonoBehaviour {

	private NetworkManager manager;

	// Use this for initialization
	void Start () {
		manager = GetComponent<NetworkManager>();
		manager.StartHost ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void OnConnected(NetworkMessage netMsg)
	{
		Debug.Log("Host activated");
	}

	void OnDestroy(){
		manager.StopHost ();
	}
}
