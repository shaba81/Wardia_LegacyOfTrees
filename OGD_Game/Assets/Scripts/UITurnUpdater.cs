using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UITurnUpdater : Manager<UITurnUpdater>
{
    public Text turn;
    private int currentTurn = 0;

    public void UpdateTurn()
    {
        currentTurn = GameManager.Instance.GetTurnCounter();
        turn.text = currentTurn.ToString();
    }
}
