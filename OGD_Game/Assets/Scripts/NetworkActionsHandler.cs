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
        Debug.LogFormat("spawned enemy in :" +  index.ToString());
    }
    public void SendEntity(string name, int index)
    {
        this.photonView.RPC("SpawnEntity", RpcTarget.Others, name, index);
    }


    [PunRPC]
     void ChangeTurn()
    {
        TurnManager.Instance.SetGameState(GameState.Start);
        Debug.LogFormat("Changed turn");
    }
    public void SendTurn()
    {
        this.photonView.RPC("ChangeTurn", RpcTarget.Others);
    }



}
