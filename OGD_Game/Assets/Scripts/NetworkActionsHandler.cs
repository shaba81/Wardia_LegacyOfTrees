using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkActionsHandler : MonoBehaviourPunCallbacks
{


    [PunRPC]
    void SpawnEntity(string name, int index)
    {
        EnemySpawner.Instance.SpawnEnemy(name, index);
    }
    public void SendEntity(string name, int index)
    {
        this.photonView.RPC("SpawnEntity", RpcTarget.Others, name, index);
    }

    private void OnApplicationQuit() {
        Debug.Log("Disconnecting....");
        PhotonNetwork.LeaveRoom();
        GameManager.Instance.ResetAll();
        Debug.Log("LeavingRoom....");
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");
        PhotonNetwork.SendAllOutgoingCommands();
    
    }
     public override void OnDisconnected(DisconnectCause cause)
    {
        GameManager.Instance.ResetAll();
        Debug.Log("Resetting Game Manager");
        LevelLoader.Instance.LoadMainmenu();
        Debug.Log("Loading Main Menu");
        Debug.LogWarningFormat("PUN: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    [PunRPC]
    void ChangeTurn()
    {
        GameManager.Instance.FireOpponentActions();
        TurnManager.Instance.SetGameState(GameState.Start);
    }


    public void SendTurn()
    {
        this.photonView.RPC("ChangeTurn", RpcTarget.Others);
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        Disconnect(false);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

        }
    }

    public void Disconnect(bool isMe)
    {
        if (!isMe)
        {
            //messaggio: player left the room
        }
        else
        {
            //messaggio: disconnected from server
        }

        PhotonNetwork.Disconnect();
    }
    [PunRPC]
    void UpdateTurn()
    {
        GameManager.Instance.currentTurn += 1;
        UITurnUpdater.Instance.UpdateTurn();
    }
    public void SendUpdateTurn()
    {
        this.photonView.RPC("UpdateTurn", RpcTarget.Others);
    }


    [PunRPC]
    void UpdateOpponentNaturePoints(int amount)
    {
        GameManager.Instance.UpdateOpponentNaturePoints(amount);
        //Debug.LogFormat("Updated Nature Points");
    }
    public void SendNaturePoints(int amount)
    {
        this.photonView.RPC("UpdateOpponentNaturePoints", RpcTarget.Others, amount);
    }

    [PunRPC]
    void UpdateTrees()
    {
        GameManager.Instance.UpdateEnemyTrees();
        //Debug.LogFormat("Updated Enemy Trees");
    }
    public void SendTreeUpdate()
    {
        this.photonView.RPC("UpdateTrees", RpcTarget.Others);
    }


}
