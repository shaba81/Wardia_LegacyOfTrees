using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Manager<EnemySpawner>
{
    public Team opposingTeam;
    private Transform parent;
    public Transform enemySpawn;
    public EntitiesDatabaseSO database;
    private EntitiesDatabaseSO.EntityData entityData;

    void Start()
    {
        if(TeamManager.Instance.GetTeam() == Team.Team1)
            opposingTeam = Team.Team2;
        else if (TeamManager.Instance.GetTeam() == Team.Team2)
            opposingTeam = Team.Team1;

        Debug.LogFormat(opposingTeam.ToString());

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
        newEntity.GetComponent<Dragger>().enabled = false;

        newEntity.gameObject.name = entityData.name;
        newEntity.movement = entityData.movement;
        newEntity.baseHealth = entityData.health;
        newEntity.baseDamage = entityData.damage;
        newEntity.isBuilding = entityData.isBuilding;

        GameManager.Instance.GetEntitiesAgainst(GameManager.Instance.myTeam).Add(newEntity);

        Node node = GridManager.Instance.GetNodeAtIndex(nodeIndex);

        if(node.IsOccupied)
        {
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(GameManager.Instance.myTeam))
            {
                if (entity.CurrentNode == node)
                {
                    entity.TakeDamage(entity.baseHealth);
                    entity.RedParticlesPlay();
                    entity.HideEntity();
                    break;
                }
            }
        }

        newEntity.SetCurrentNode(node);
        newEntity.SetStartingNode(node);
        GridManager.Instance.GetNodeAtIndex(nodeIndex).SetOccupied(true);

        newEntity.Setup(opposingTeam, node.worldPosition);
        newEntity.WhiteParticlesPlay();
    }
}
