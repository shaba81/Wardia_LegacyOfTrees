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
        myTeam = Team.Team1;
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
            PlayerData.Instance.GiveMoney();

            foreach (TreeEntity tree in GameManager.Instance.trees)
            {

                foreach (BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam)) {

                    if(tree.parent == GridManager.Instance.GetTileForNode(entity.CurrentNode)) {
                        tree.SetConquerer(myTeam);
                    }
                }

                if(tree.GetConquerer() == myTeam)
                {
                    PlayerData.Instance.GiveMoney();
                }
            }
            UITreeUpdater.Instance.UpdateTrees();
            tm.SetGameState(GameState.Buying);
        }


        
    }

    public void HandleOnStateChange()
    {
        turnText.text = tm.gameState.ToString();
    }

    public void EndTurn()
    {
        GameManager.Instance.StartActions();
        tm.SetGameState(GameState.Start);
    }

}
