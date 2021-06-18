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

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();
    List<BaseEntity> allEntities = new List<BaseEntity>();
    private int team1builderCounter = 0;
    private int team2builderCounter = 0;
    public List<TreeEntity> trees = new List<TreeEntity>();
    private List<BaseEntity> toRemove = new List<BaseEntity>();

    private List<Node> builderNodes = new List<Node>();


    int currentTurn = 1;

    private void Start() {
        myTeam = TeamManager.Instance.GetTeam();
    }
    public void OnEntityBought(EntitiesDatabaseSO.EntityData entityData)
    {
        Transform parent = team1Parent;

        if (myTeam == Team.Team1)
            parent = team1Parent;
        else if(myTeam == Team.Team2)
            parent = team2Parent;

        BaseEntity newEntity = Instantiate(entityData.prefab, parent);

            newEntity.gameObject.name = entityData.name;
            newEntity.movement = entityData.movement;
            newEntity.baseHealth = entityData.health;
            newEntity.baseDamage = entityData.damage;
        newEntity.isBuilding = entityData.isBuilding;

        if(myTeam == Team.Team1)
            team1Entities.Add(newEntity);
        else if(myTeam == Team.Team2)
            team2Entities.Add(newEntity);



        newEntity.Setup(myTeam, /*GridManager.Instance.GetFreeNode(Team.Team1)*/ spawnTransform.position);

            TurnManager.Instance.SetGameState(GameState.Placing);
        
    }

    public Team GetOpposingTeam()
    {
        if (myTeam == Team.Team1)
            return Team.Team2;
        else if (myTeam == Team.Team2)
            return Team.Team1;

        return Team.None;
    }

    public void SortEntities()
    {
        team1Entities.Sort((x, y) => y.CurrentNode.index.CompareTo(x.CurrentNode.index)); // descending, for the opponent. Just to remind
        team2Entities.Sort((x, y) => x.CurrentNode.index.CompareTo(y.CurrentNode.index)); // asc
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
        if(team == Team.Team1)
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

        foreach(TreeEntity t in trees)
        {
            if (t.GetConquerer() == team)
                amount += 1;
        }

        return amount;
    }

    public void IncreaseBuilderCounter(Team team)
    {
        if(team == Team.Team1)
        {
            team1builderCounter += 1;
        }
        else if (team == Team.Team2)
        {
            team2builderCounter += 1;
        }
    }

    public void DecreaseBuilderCounter(Team team)
    {
        if (team == Team.Team1)
        {
            team1builderCounter -= 1;
        }
        else if (team == Team.Team2)
        {
            team2builderCounter -= 1;
        }
    }

    public bool GetBuilderCounter(Team team)
    {
        if (team == Team.Team1)
        {
            if (team1builderCounter > 0)
                return true;
            else
                return false;
        }
        else if (team == Team.Team2)
        {
            if (team2builderCounter > 0)
                return true;
            else
                return false;
        }

        return false;
    }

    public void UpdateTurnCounter()
    {
        currentTurn += 1;
    }

    public int GetTurnCounter()
    {
        return currentTurn;
    }

    public Team GetTroopForNode(Node node)
    {
        foreach(BaseEntity entity in team1Entities.Concat(team2Entities))
        {
            if (entity.CurrentNode == node)
                return entity.GetMyTeam();
        }

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


    public void FireRoundEndActions ()
    {
        //OnRoundEnd?.Invoke(); 
        StartCoroutine(RoundEndCoroutine(GetMyEntities(myTeam)));      
    }

    public void FireRoundStartActions()
    {
        OnRoundStart?.Invoke();
    }


    public void UnitDead(BaseEntity entity)
    {
        //team1Entities.Remove(entity);
        //team2Entities.Remove(entity);

        toRemove.Add(entity);

        OnUnitDied?.Invoke(entity);

        entity.Unsubscribe();

        Destroy(entity.gameObject);
    }

    public void ChangeTeam()
    {
        if (myTeam == Team.Team1)
            myTeam = Team.Team2;
        else if (myTeam == Team.Team2)
            myTeam = Team.Team1;
    }

    public bool CheckTurnLimit()
    {
        if (GetTurnCounter() == 20)
            return true;
        else
            return false;
    }

    /*
    public void DebugFight()
    {
        for (int i = 0; i < unitsPerTeam; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, entitiesDatabase.allEntities.Count);
            BaseEntity newEntity = Instantiate(entitiesDatabase.allEntities[randomIndex].prefab, team2Parent);

            team2Entities.Add(newEntity);

            newEntity.Setup(Team.Team2, GridManager.Instance.GetFreeNode(Team.Team2));
        }
    }
    */

    IEnumerator RoundEndCoroutine(List<BaseEntity> _entities)
    {
        //TurnManager.Instance.SetGameState(GameState.EndTurn);

        WaitForSeconds wait = new WaitForSeconds(0.5f);

        List<BaseEntity> tempList = _entities;

        foreach (BaseEntity e in tempList.ToList())
        {
            if(!e.isFirstTurn)
            {
                e.OnRoundEnd();
                yield return wait;
            }
            else
            {
                e.isFirstTurn = false;
            }

        }

        foreach(BaseEntity remove in toRemove)
        {
            team1Entities.Remove(remove);
            team2Entities.Remove(remove);
        }

        toRemove.Clear();
    }
}

public enum Team
{
    Team1,
    Team2,
    None
}
