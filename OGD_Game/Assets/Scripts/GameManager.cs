using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
public class GameManager : Manager<GameManager>
{
    public EntitiesDatabaseSO entitiesDatabase;
    public EntitiesDatabaseSO buildingsDatabase;

    public Transform team1Parent;
    public Transform team2Parent;
    public Transform spawnTransform;

    public Action OnRoundStart;
    public Action OnRoundEnd;
    public Action<BaseEntity> OnUnitDied;

    public Team myTeam;
    private Team winner = Team.None;

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();
    List<BaseEntity> allEntities = new List<BaseEntity>();
    public List<TreeEntity> trees = new List<TreeEntity>();
    private List<BaseEntity> toRemove = new List<BaseEntity>();

    private List<Node> builderNodes = new List<Node>();

    [SerializeField]
    public NetworkActionsHandler networkActionsHandler;
    public int currentTurn = 1;
    private int allTrees;
    public int opponentNaturePoints;

    private void Start()
    {
        Destroy(GameObject.Find("AudioSource"));
        myTeam = TeamManager.Instance.GetTeam();
        if (myTeam == Team.Team2)
        {
            Vector3 tempParentPosition = team1Parent.position;
            team1Parent.position = team2Parent.position;
            team2Parent.position = tempParentPosition;
        }
        allTrees = trees.Count;

    }
    public void OnEntityBought(EntitiesDatabaseSO.EntityData entityData)
    {
        Transform parent = team1Parent;

        if (myTeam == Team.Team1)
            parent = team1Parent;
        else if (myTeam == Team.Team2)
            parent = team2Parent;

        BaseEntity newEntity = Instantiate(entityData.prefab, parent);

        newEntity.gameObject.name = entityData.name;
        newEntity.movement = entityData.movement;
        newEntity.baseHealth = entityData.health;
        newEntity.baseDamage = entityData.damage;
        newEntity.isBuilding = entityData.isBuilding;
        newEntity.cost = entityData.cost;

        if (myTeam == Team.Team1)
            team1Entities.Add(newEntity);
        else if (myTeam == Team.Team2)
            team2Entities.Add(newEntity);



        newEntity.Setup(myTeam, /*GridManager.Instance.GetFreeNode(Team.Team1)*/ spawnTransform.position);

        TurnManager.Instance.SetGameState(GameState.Placing);

    }

    public void RevertSpawn()
    {
        foreach(Tile _t in GridManager.Instance.GetAllTiles())
        {
            _t.SetEligibleHighlight(false);
        }

        foreach(BaseEntity entity in GetMyEntities(myTeam))
        {
            if(entity.CurrentNode == null)
            {
                PlayerData.Instance.GiveMoney(entity.cost);
                Remove(entity);
                TurnManager.Instance.SetGameState(GameState.Buying);
                break;
            }
        }
    }

    public Team GetOpposingTeam()
    {
        if (myTeam == Team.Team1)
            return Team.Team2;
        else if (myTeam == Team.Team2)
            return Team.Team1;

        return Team.None;
    }

    public bool CheckVictoryByTrees()
    {

        if (GetTreesConquered(myTeam) == 5)
            return true;

        return false;
    }

    public void UpdateOpponentNaturePoints(int amount)
    {
        opponentNaturePoints = amount;
        Debug.Log("OPPONENT NATURE POINTS: " + opponentNaturePoints);
    }

    public void UpdateEnemyTrees()
    {

        foreach (TreeEntity tree in trees)
        {
            foreach (BaseEntity entity in GetEntitiesAgainst(myTeam))
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

    public void SortEntities()
    {
        team1Entities.Sort((x, y) => y.CurrentNode.index.CompareTo(x.CurrentNode.index)); // descending, for the opponent. Just to remind
        team2Entities.Sort((x, y) => x.CurrentNode.index.CompareTo(y.CurrentNode.index)); // asc
    }

    public Team GetWinner()
    {
        return winner;
    }

    public void SetWinner(Team _winner)
    {
        winner = _winner;
    }

    public List<BaseEntity> GetEntitiesAgainst(Team myTeam)
    {
        if (myTeam == Team.Team1)
            return team2Entities;
        else if (myTeam == Team.Team2)
            return team1Entities;

        return null;
    }

    public BaseEntity GetEntityAtIndex(Team team, int index)
    {
        if (team == Team.Team1)
        {
            return team1Entities[index];
        }
        else if (team == Team.Team2)
        {
            return team2Entities[index];
        }

        return null;
    }

    public int GetTreesConquered(Team team)
    {
        int amount = 0;

        foreach (TreeEntity t in trees)
        {
            if (t.GetConquerer() == team)
                amount += 1;
        }

        return amount;
    }

    public bool CheckBuilders(Team _team)
    {
        bool builderFound = false;

        if(_team == Team.Team1)
        {
            foreach (BaseEntity entity in team1Entities)
            {
                if(entity != null && entity.isBuilder)
                {
                    builderFound = true;
                    break;
                }
            }

        }
        else if (_team == Team.Team2)
        {
            foreach (BaseEntity entity in team2Entities)
            {
                if (entity != null)
                {
                    builderFound = true;
                    break;
                }
            }

        }

        return builderFound;
    }


    public int GetTurnCounter()
    {
        return currentTurn;
    }

    public Team GetTroopForNode(Node node)
    {
        foreach (BaseEntity entity in GetAllEntities())
        {
            if (entity != null && entity.CurrentNode == node && !entity.dead)
                return entity.GetMyTeam();
        }


        // Debug.Log("NONE");
        return Team.None;
    }

    public List<BaseEntity> GetAllEntities()
    {
        allEntities.Clear();
        foreach (BaseEntity entity in team1Entities.Concat(team2Entities))
        {
            allEntities.Add(entity);
        }
        return allEntities;
    }

    public bool checkTreeRequirement(int required)
    {
        if (GetTreesConquered(myTeam) < required)
            return false;

        return true;
    }

    public List<BaseEntity> GetMyEntities(Team team)
    {
        if (team == Team.Team1)
            return team1Entities;
        else if (team == Team.Team2)
            return team2Entities;

        return null;
    }


    public void FireRoundEndActions()
    {
        StartCoroutine(RoundEndCoroutine(GetMyEntities(myTeam)));
    }

    public void FireOpponentActions()
    {
        StartCoroutine(RoundEndCoroutine(GetEntitiesAgainst(myTeam)));
    }

    public void FireRoundStartActions()
    {
        OnRoundStart?.Invoke();
    }


    public void UnitDead(BaseEntity entity)
    {

        toRemove.Add(entity);

    }

    public bool CheckTurnLimit()
    {
        if (GetTurnCounter() == 20)
            return true;
        else
            return false;
    }

    IEnumerator RoundEndCoroutine(List<BaseEntity> _entities)
    {
        //TurnManager.Instance.SetGameState(GameState.EndTurn);

        WaitForSeconds wait = new WaitForSeconds(0.5f);

        List<BaseEntity> tempList = _entities;

        foreach (BaseEntity e in tempList)
        {
            if (!e.isFirstTurn)
            {
                e.OnRoundEnd();
                yield return wait;
            }
            else
            {
                e.isFirstTurn = false;
            }

        }

        foreach (BaseEntity remove in toRemove)
        {
            Remove(remove);
        }

        toRemove.Clear();
    }

    public void Remove(BaseEntity remove)
    {

        team1Entities.Remove(remove);

        team2Entities.Remove(remove);

        OnUnitDied?.Invoke(remove);

        remove.Unsubscribe();

        Destroy(remove.gameObject);
    }

    // public void RemoveAt(int nodeIndex)
    // {
    //     foreach (BaseEntity e in team1Entities)
    //     {
    //         if (e.CurrentNode.index == nodeIndex)
    //         {
    //             team1Entities.Remove(e);
    //             OnUnitDied?.Invoke(e);

    //             e.Unsubscribe();

    //             Destroy(e.gameObject);
    //             break;
    //         }
    //     }
    //     foreach (BaseEntity e in team2Entities)
    //     {
    //         if (e.CurrentNode.index == nodeIndex)
    //         {
    //             team2Entities.Remove(e);
    //             OnUnitDied?.Invoke(e);

    //             e.Unsubscribe();

    //             Destroy(e.gameObject);
    //             break;
    //         }
    //     }
    // }




    // public void FireOnRoundEndAt(int nodeIndex)
    // {
    //     allEntities = GetAllEntities();
    //     foreach (BaseEntity e in allEntities)
    //     {
    //         if (e.CurrentNode.index == nodeIndex)
    //         {
    //             Debug.Log("Calling on Round end at index  - " + nodeIndex.ToString());
    //             e.OnRoundEnd();
    //             break;
    //         }
    //     }
    // }

    public void ResetAll()
    {
        myTeam = Team.None;
        GridManager.Instance.ResetNodes();

        foreach(BaseEntity entity in GetAllEntities())
        {
            Remove(entity);
        }
        foreach(TreeEntity tree in trees)
        {
            tree.isConquered = false;
            tree.SetConquerer(Team.None);
        }

        toRemove.Clear();
        winner = Team.None;
        currentTurn = 1;
        opponentNaturePoints = 0;
        PlayerData.Instance.ResetMoney();
    }
}

public enum Team
{
    Team1,
    Team2,
    None
}
