using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook {

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		MyPlayerController myplayer = gamePlayer.GetComponent<MyPlayerController>();

		myplayer.playerName = lobby.playerName;
		myplayer.playerColor = lobby.playerColor;
		if(myplayer.playerColor == Color.red)
			myplayer.teamNumber = 1;
		else
			myplayer.teamNumber = 2;

		// custom
		myplayer.lifePoint = 2;

	}

}
