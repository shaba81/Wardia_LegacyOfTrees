using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvangelistEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        //Retrieve current node
        //Check for neighbors and check all entities that have the same node as the neighbors
        //Give Buffs

        foreach (Node n in GridManager.Instance.Neighbors(currentNode))
        {
            foreach (BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam))
            {
                if(entity != null)
                {
                    if (entity.name.Contains("Peasant") && entity.CurrentNode == n)
                    {
                        if (!entity.IsHealthBuffed())
                        {
                            entity.ReceiveDefenseBuff(1);
                            PopUpManager.Instance.SpawnPopUp(entity.transform.position, PopUpType.Health);
                            Debug.Log("Entity: " + entity.name + " received buff." + entity.baseHealth);
                        }
                    }
                }
            }
        }

    }

    public override void OnRoundEnd()
    {
        if (!Move(movement))
        {
            //means there's someone on the other tile.
            //if it's an enemy, combat;
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                Debug.Log(entity.GetMyTeam());
                if (GridManager.Instance.GetNextNode(currentNode, positions, false, myTeam).index == entity.CurrentNode.index)
                {
                        Debug.Log("Combat");
                        int damageToTake = entity.baseDamage;

                        entity.TakeDamage(baseDamage);

                        //This means if i'm not dead
                        if (!TakeDamage(damageToTake))
                            Move(movement);

                  
                }
            }
        }
    }
}
