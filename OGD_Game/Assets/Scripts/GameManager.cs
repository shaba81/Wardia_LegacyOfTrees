using System.Collections;
using System.Collections.Generic;
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

    public Team myTeam = Team.Team1;

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();
    public List<TreeEntity> trees = new List<TreeEntity>();


    //int unitsPerTeam = 6;

    public void OnEntityBought(EntitiesDatabaseSO.EntityData entityData)
    {
            BaseEntity newEntity = Instantiate(entityData.prefab, team1Parent);
            newEntity.gameObject.name = entityData.name;
            newEntity.movement = entityData.movement;
            newEntity.baseHealth = entityData.health;
            newEntity.baseDamage = entityData.damage;
        newEntity.isBuilding = entityData.isBuilding;
        team1Entities.Add(newEntity);

            newEntity.Setup(myTeam, /*GridManager.Instance.GetFreeNode(Team.Team1)*/ spawnTransform.position);

            TurnManager.Instance.SetGameState(GameState.Placing);
        
    }

    public List<BaseEntity> GetEntitiesAgainst(Team against)
    {
        if (against == Team.Team1)
            return team2Entities;
        else
            return team1Entities;
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
        else
            return team2Entities;
    }


    public void SetupActions ()
    {
        OnRoundStart?.Invoke(); 
    }


    public void UnitDead(BaseEntity entity)
    {
        team1Entities.Remove(entity);
        team2Entities.Remove(entity);

        OnUnitDied?.Invoke(entity);

        Destroy(entity.gameObject);
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
}

public enum Team
{
    Team1,
    Team2,
    None
}
