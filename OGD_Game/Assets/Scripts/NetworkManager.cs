using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{


    [SerializeField]
    private GameObject progressLabel;

    [SerializeField]
    private Text opponentName;

    [SerializeField]
    private Text playerName;

    [SerializeField]
    private AnimationMatchFound matchFoundScript;

    string gameVersion = "1";

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        playerName.gameObject.SetActive(false);
        progressLabel.SetActive(false);
    }


    public void Connect()
    {
        matchFoundScript.DeleteButtons();
        progressLabel.SetActive(true);
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
        Debug.LogFormat("PlayerEnteredRoom Name:" + other.NickName);
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {

            Debug.Log("2 PLAYERS REACHED, LOADING ROOM");
            LoadArena();


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

    public void ChangePlayerName(string newName)
    {
        this.playerName.text = newName;
    }
    #region Private Methods


    void LoadArena()
    {
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        playerName.gameObject.SetActive(true);
        opponentName.text = PhotonNetwork.PlayerListOthers[0].NickName;
        opponentName.gameObject.SetActive(true);
        if (!PhotonNetwork.IsMasterClient)
        {

            matchFoundScript.playerwhite = false;
            Debug.LogError("I'm Player 2");
            TeamManager.Instance.SetTeam(Team.Team2);
        }
        else
        {

            matchFoundScript.playerwhite = true;

            Debug.LogError("I'm Player 1");
            TeamManager.Instance.SetTeam(Team.Team1);
        }
        progressLabel.SetActive(false);
        Debug.LogFormat("LOADING GAME");

        matchFoundScript.MatchFoundCoroutine();

    }


    #endregion


}
