using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    TurnManager tm;
    public Text turnText;
    public Team myTeam;
    private int opponentNaturePoints = 0;
    public LevelLoader levelLoader;
    [SerializeField]
    public NetworkActionsHandler networkActionsHandler;
    private void Awake()
    {
        tm = TurnManager.Instance;
        tm.OnStateChange += HandleOnStateChange;
    }

    void Start()
    {
        myTeam = GameManager.Instance.myTeam;
        if (myTeam == Team.Team1)
        {
            tm.SetGameState(GameState.Start);
        }
        else if (myTeam == Team.Team2)
        {
            TurnManager.Instance.SetGameState(GameState.Wait);
        }
        turnText.text = tm.gameState.ToString();
    }


    void Update()
    {
        //CHECK IF THE PLAYER HAS ALL THE TREES
        if (GameManager.Instance.CheckVictoryByTrees())
        {
            GameManager.Instance.SetWinner(GameManager.Instance.myTeam);
            Debug.Log(GameManager.Instance.GetWinner() + " won the match!");
            levelLoader.LoadResultscreen();
        }

        //CHECK IF WE REACHED TURN LIMIT
        if (GameManager.Instance.CheckTurnLimit())
        {
            GameManager.Instance.SetWinner(UITreeUpdater.Instance.GetWinner());

            //check who has more trees
            if (GameManager.Instance.GetWinner() == GameManager.Instance.myTeam)
            {
                Debug.Log(GameManager.Instance.GetWinner() + " won the match!");
                levelLoader.LoadResultscreen();
                //SWITCH TO END SCREEN, variable winner is preserved.
            }
            else if (GameManager.Instance.GetWinner() != GameManager.Instance.myTeam && GameManager.Instance.GetWinner() != Team.None)
            {
                Debug.Log(GameManager.Instance.GetWinner() + " won the match! And you lost.");
                levelLoader.LoadResultscreen();
                //SWITCH TO END SCREEN, variable winner is preserved.
            }
            else
            {
                Debug.Log("Same amount of trees!");
            }


            //if they're equal, check who whas more nature points
            networkActionsHandler.SendNaturePoints(PlayerData.Instance.Money);
            opponentNaturePoints = GameManager.Instance.opponentNaturePoints;
            if (PlayerData.Instance.Money > opponentNaturePoints)
            {
                GameManager.Instance.SetWinner(GameManager.Instance.myTeam);
                Debug.Log(GameManager.Instance.GetWinner() + " won the match!");
                levelLoader.LoadResultscreen();
                //SWITCH TO END SCREEN, variable winner is preserved.
            }
            else if (PlayerData.Instance.Money < opponentNaturePoints)
            {
                GameManager.Instance.SetWinner(GameManager.Instance.GetOpposingTeam());
                Debug.Log(GameManager.Instance.GetWinner() + " won the match! And you lost.");
                levelLoader.LoadResultscreen();
                //SWITCH TO END SCREEN, variable winner is preserved.
            }
        }

        if (tm.gameState == GameState.Start)
        {
            UIButtonManager.Instance.EnableEndButton();

            
            PlayerData.Instance.GiveMoney(2);

            foreach (TreeEntity tree in GameManager.Instance.trees)
            {

                foreach (BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam))
                {
                    //entity.isFirstTurn = false;
                    if (tree.parent == GridManager.Instance.GetTileForNode(entity.CurrentNode))
                    {
                        tree.SetConquerer(entity.GetMyTeam());
                    }
                }


                if (tree.GetConquerer() == myTeam)
                {
                    PlayerData.Instance.GiveMoney(1);
                    PopUpManager.Instance.SpawnPopUp(tree.transform.position, PopUpType.OnePoint);
                }
            }
            networkActionsHandler.SendTreeUpdate();
            UITreeUpdater.Instance.UpdateTrees();

            //Call on round start for each entity and do the actions
            GameManager.Instance.FireRoundStartActions();

            tm.SetGameState(GameState.Buying);
        }

        if (tm.gameState == GameState.EndTurn)
        {
            UIButtonManager.Instance.DisableEndButton();
            GameManager.Instance.SortEntities();
            GameManager.Instance.FireRoundEndActions();



            UpdateTurnCounter();
            UITurnUpdater.Instance.UpdateTurn();

            networkActionsHandler.SendTurn();
            TurnManager.Instance.SetGameState(GameState.Wait);
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

    public void UpdateTurnCounter()
    {
        GameManager.Instance.currentTurn += 1;
        networkActionsHandler.SendUpdateTurn();
    }

    public void UpdateEnemyTrees()
    {
        foreach (TreeEntity tree in GameManager.Instance.trees)
        {

            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                //entity.isFirstTurn = false;
                if (tree.parent == GridManager.Instance.GetTileForNode(entity.CurrentNode))
                {
                    tree.SetConquerer(entity.GetMyTeam());
                }
            }
        }
        UITreeUpdater.Instance.UpdateTrees();
    }

}
