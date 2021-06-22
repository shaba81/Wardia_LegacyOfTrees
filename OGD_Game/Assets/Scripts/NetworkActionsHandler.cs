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

    // [PunRPC]
    // void OnRoundEnd(int nodeIndex)
    // {
    //     GameManager.Instance.FireOnRoundEndAt(nodeIndex);
    //     // Debug.Log("Round End");
    // }
    // public void FireOnRoundEnd(BaseEntity e)
    // {
    //     this.photonView.RPC("OnRoundEnd", RpcTarget.All, e.CurrentNode.index);
    // }

    // [PunRPC]
    // void RemoveEntity(int index)
    // {
    //     GameManager.Instance.RemoveAt(index);
    //     Debug.Log("REMOVED ENTITY");
    // }
    // public void FireRemoveEntity(BaseEntity remove)
    // {
    //     this.photonView.RPC("RemoveEntity", RpcTarget.Others, remove.CurrentNode.index);
    // }

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
