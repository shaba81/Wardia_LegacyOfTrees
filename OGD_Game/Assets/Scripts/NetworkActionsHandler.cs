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
        Debug.Log("LeavingRoom....");
        PhotonNetwork.Disconnect();
        Debug.Log("Disconnected");
        PhotonNetwork.SendAllOutgoingCommands();
    
    }
     public override void OnDisconnected(DisconnectCause cause)
    {
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
        if(!GameManager.Instance.gameEnded)
            LevelLoader.Instance.LoadDisconnectScene();
        Disconnect(false);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

        }
    }

    public void Disconnect(bool isMe)
    {
        PhotonNetwork.Disconnect();
    }

     public void DisconnectToMainMenu(bool isMe)
    {
        PhotonNetwork.Disconnect();
        LevelLoader.Instance.LoadMainmenu();
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
