using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class NetworkManager : MonoBehaviourPunCallbacks
{


    [SerializeField]
    private GameObject controlPanel;
    [SerializeField]
    private GameObject progressLabel;

    [SerializeField]
    private GameObject gamePanelSample;

    [SerializeField]
    private GameObject textSample;

    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        gamePanelSample.SetActive(false);
        textSample.SetActive(false);
    }


    public void Connect()
    {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #region MonoBehaviourPunCallbacks Callbacks


    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN: OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("PUN: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN :OnJoinRandomFailed() was called by PUN. No random room available, so one will be created.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            LoadArena();
            Debug.Log("2 PLAYERS REACHED, LOADING ROOM");

        }
    }


    #endregion


    #region Photon Callbacks


    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            LoadArena();
            Debug.Log("2 PLAYERS REACHED, LOADING ROOM");

        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

        }
    }


    #endregion

    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("I'm Player 2");
        }
        else
        {
            Debug.LogError("I'm Player 1");
        }
        progressLabel.SetActive(false);
        gamePanelSample.SetActive(true);
        Debug.LogFormat("LOADING GAME");

        // TODO: CARICO IL LIVELLO E SETTO I PLAYER
        // int numPlayers = 2;
        // PhotonNetwork.LoadLevel(numPlayers);
    }


    #endregion

    #region Sample actions Methods

    [PunRPC]
     void sampleAction(string test)
    {
        textSample.SetActive(true);
        Debug.LogFormat("received message:" +  test);
    }
    public void SendAction()
    {
        this.photonView.RPC("sampleAction", RpcTarget.Others, "ciao");
    }

    #endregion

}
