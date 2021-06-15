using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Manager<EnemySpawner>
{
    private Team opposingTeam;
    private Transform parent;
    public Transform enemySpawn;
    public EntitiesDatabaseSO database;
    private EntitiesDatabaseSO.EntityData entityData;

    void Start()
    {
        opposingTeam = GameManager.Instance.GetOpposingTeam();
        if(opposingTeam == Team.Team1)
        {
            parent = GameManager.Instance.team1Parent;
        } 
        else if (opposingTeam == Team.Team2)
        {
            parent = GameManager.Instance.team2Parent;
        }
    }

    //QUALE ENTITY SPAWNARE
    public void SpawnEnemy(string name, int nodeIndex)
    {
        
        foreach (EntitiesDatabaseSO.EntityData data in database.allEntities)
        {
            if (data.name.Equals(name))
            {
                entityData = data;
            }
                
        }

        BaseEntity newEntity = Instantiate(entityData.prefab, parent);

        newEntity.gameObject.name = entityData.name;
        newEntity.movement = entityData.movement;
        newEntity.baseHealth = entityData.health;
        newEntity.baseDamage = entityData.damage;
        newEntity.isBuilding = entityData.isBuilding;

        GameManager.Instance.GetMyEntities(opposingTeam).Add(newEntity);

        Node node = GridManager.Instance.GetNodeAtIndex(nodeIndex);

        newEntity.SetCurrentNode(node);
        newEntity.SetStartingNode(node);
        GridManager.Instance.GetNodeAtIndex(nodeIndex).SetOccupied(true);

        newEntity.Setup(opposingTeam, node.worldPosition);
    }
}
