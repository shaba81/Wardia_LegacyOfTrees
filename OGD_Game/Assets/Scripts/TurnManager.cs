using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Buying, Placing, EndTurn, Wait}
/*
 * Buying State --> Keep in that state until satisfied or can't afford
 * Placing State --> When a troop is picked, back to buying state when it's placed.
 * EndTurn --> When the end button is pressed, then all troops make their action.
 * Wait Turn --> When all troops have done their action. Back to buying state when opponent has ended their turn.
 *
 */

public delegate void OnStateChangeHandler();

public class TurnManager
{
    private static TurnManager _instance = null;
    public GameState gameState { get; private set; }
    public event OnStateChangeHandler OnStateChange;

    protected TurnManager()
    { }
    
        public static TurnManager Instance {
            get {
                if (_instance == null) {
                _instance = new TurnManager();
                }
            return _instance;
            }     
        }



    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
    }
}
