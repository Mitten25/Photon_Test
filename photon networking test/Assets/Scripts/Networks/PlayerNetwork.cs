using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerNetwork : MonoBehaviour {

    public static PlayerNetwork instance;
    public string PlayerName { get; private set; }
    private PhotonView PhotonView;
    private int PlayersInGame = 0;

    private PlayerMovement CurrentPlayer;

	void Awake () {
        instance = this;
        PhotonView = GetComponent<PhotonView>();

        PlayerName = "Mitten#" + Random.Range(1000, 9999);

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 30;

        SceneManager.sceneLoaded += OnSceneFinishedLoading;
	}
	
	private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            if (PhotonNetwork.isMasterClient)
                MasterLoadedGame();
            else
                NonMasterLoadedGame();
        }
    }

    private void MasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
        PhotonView.RPC("RPC_LoadGameOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        PhotonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient, PhotonNetwork.player);
    }

    [PunRPC]
    private void RPC_LoadGameOthers()
    {
        PhotonNetwork.LoadLevel(1);
    }

    [PunRPC]
    private void RPC_LoadedGameScene(PhotonPlayer photonPlayer)
    {
        PlayerManagement.instance.AddPlayerStats(photonPlayer);

        PlayersInGame++;
        if(PlayersInGame == PhotonNetwork.playerList.Length)
        {
            print("All Players are In");
            PhotonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    public void NewHealth(PhotonPlayer photonPlayer, int health)
    {
        PhotonView.RPC("RPC_NewHealth", photonPlayer, health);
    }

    [PunRPC]
    private void RPC_NewHealth(int health)
    {
        if (CurrentPlayer == null)
            return;

        if (health <= 0)
        {
            PhotonNetwork.Destroy(CurrentPlayer.gameObject);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        float randomValue = Random.Range(0, 5);
        GameObject obj = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "NewPlayer"), Vector3.up * randomValue, Quaternion.identity, 0);
        CurrentPlayer = obj.GetComponent<PlayerMovement>();
    }
}
