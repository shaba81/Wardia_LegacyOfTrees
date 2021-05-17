using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    TurnManager tm;
    public Text turnText;

    private void Awake()
    {
        tm = TurnManager.Instance;
        tm.OnStateChange += HandleOnStateChange;
        tm.SetGameState(GameState.Buying);
        turnText.text = tm.gameState.ToString();
        //set opponent game state to wait. Thru network.

    }
    
    void Start()
    {
    }


    void Update()
    {
        
    }

    public void HandleOnStateChange()
    {
        turnText.text = tm.gameState.ToString();
    }

    public void EndTurn()
    {
        /*
        int currentState = (int) tm.gameState;
        int next = currentState + 1;
        GameState nextState = GameState.Buying;
        if(next < 4)
        {
            nextState = (GameState) next;
        }
        tm.SetGameState(nextState);
        */

        tm.SetGameState(GameState.EndTurn);
        Debug.Log(tm.gameState);
    } 
}
