using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    TurnManager tm;
    public Text turnText;
    public Team myTeam;

    private void Awake()
    {
        myTeam = GameManager.Instance.myTeam;
        tm = TurnManager.Instance;
        tm.OnStateChange += HandleOnStateChange;
        tm.SetGameState(GameState.Start);
        turnText.text = tm.gameState.ToString();
        //set opponent game state to wait. Thru network.

    }
    
    void Start()
    {
    }


    void Update()
    {
        if(tm.gameState == GameState.Start)
        {
            UIButtonManager.Instance.EnableEndButton();

            //CHECK IF THE PLAYER HAS ALL THE TREES
            // if(...)

            //CHECK IF WE REACHED TURN LIMIT
            if (GameManager.Instance.CheckTurnLimit())
            {
                //check who has more trees

                //if they're equal, check who whas more nature points

                //then switch to endGame Screen, that shows the winner etc....
            }

            PlayerData.Instance.GiveMoney(2);

            foreach (TreeEntity tree in GameManager.Instance.trees)
            {

                foreach (BaseEntity entity in GameManager.Instance.GetAllEntities()) {
                    //entity.isFirstTurn = false;
                    if(tree.parent == GridManager.Instance.GetTileForNode(entity.CurrentNode)) {
                        tree.SetConquerer(entity.GetMyTeam());
                    }
                }

                if(tree.GetConquerer() == myTeam)
                {
                    PlayerData.Instance.GiveMoney(1);
                }
            }
            UITreeUpdater.Instance.UpdateTrees();
            
            //CHECK WINNING CONDITION

            //Call on round start for each entity and do the actions
            GameManager.Instance.FireRoundStartActions();

            tm.SetGameState(GameState.Buying);
        }

        if(tm.gameState == GameState.EndTurn)
        {
            UIButtonManager.Instance.DisableEndButton();
            GameManager.Instance.SortEntities();
            GameManager.Instance.FireRoundEndActions();



            GameManager.Instance.UpdateTurnCounter();

            UITurnUpdater.Instance.UpdateTurn();
            TurnManager.Instance.SetGameState(GameState.Wait);
        }

        if(tm.gameState == GameState.Wait)
        {
            //JUST TO DEBUG A MATCH -------------------
            tm.SetGameState(GameState.Start);
            GameManager.Instance.ChangeTeam();
        }
        
    }

    public void HandleOnStateChange()
    {
        turnText.text = GameManager.Instance.myTeam.ToString();
    }

    public void EndTurn()
    {
        tm.SetGameState(GameState.EndTurn);
        
    }

}
