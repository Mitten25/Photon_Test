using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour {


    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup RoomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickJoinRoom(string roomName)
    {
        if(PhotonNetwork.JoinRoom(roomName))
        {

        }
        else
        {
            print("Join Room Failed");
        }
    }
}
