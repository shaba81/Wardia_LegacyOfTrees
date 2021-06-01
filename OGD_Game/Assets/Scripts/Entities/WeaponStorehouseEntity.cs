using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorehouseEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        //Retrieve current node
        //Check for neighbors and check all entities that have the same node as the neighbors
        //Give Buffs

        foreach(Node n in GridManager.Instance.Neighbors(currentNode))
        {
            foreach(BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam))
            {
                if(!entity.isBuilding && entity.CurrentNode == n)
                {
                    if(!entity.IsDamageBuffed())
                    {
                        entity.ReceiveAttackBuff(1);    
                    }
                    Debug.Log("Entity: " + entity.name + " received buff." + entity.baseDamage);
                }
            }
        }

    }

    protected override void OnRoundEnd()
    {
        
    }
}
