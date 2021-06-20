using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITreeUpdater : Manager<UITreeUpdater>
{
    public Text playerTrees , opponentTrees;
    private int playerAmount, opponentAmount = 0;

    public void UpdateTrees()
    {
        playerAmount = GameManager.Instance.GetTreesConquered(Team.Team1);
        opponentAmount = GameManager.Instance.GetTreesConquered(Team.Team2);
        playerTrees.text = playerAmount.ToString();
        opponentTrees.text = opponentAmount.ToString();
    }

    public Team GetWinner()
    {
        if (playerAmount > opponentAmount)
            return Team.Team1;
        else if (playerAmount < opponentAmount)
            return Team.Team2;

        return Team.None;
    }

}
