using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNetwork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Connecting to Server...");
        PhotonNetwork.ConnectUsingSettings("0.0.0");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnConnectedToMaster()
    {
        print("Connected to Master");
        PhotonNetwork.automaticallySyncScene = false;
        PhotonNetwork.playerName = PlayerNetwork.instance.PlayerName;

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Joined Lobby");

        if(!PhotonNetwork.inRoom)
            MainCanvasManager.instance.LobbyCanvas.transform.SetAsLastSibling();
    }
}
