using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITreeUpdater : Manager<UITreeUpdater>
{
    public Text playerTrees , opponentTrees;

    public void UpdateTrees()
    {
        int playerAmount = GameManager.Instance.GetTreesConquered(Team.Team1);
        int opponentAmount = GameManager.Instance.GetTreesConquered(Team.Team2);
        playerTrees.text = playerAmount.ToString();
        opponentTrees.text = opponentAmount.ToString();
    }

}
