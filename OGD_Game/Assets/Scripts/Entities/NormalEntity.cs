using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEntity : BaseEntity
{
    public override void OnRoundEnd()
    {
        if (!Move())
        {
            //means there's someone on the other tile.
            //if it's an enemy, combat;
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                Debug.Log(entity.GetMyTeam());
                if (GridManager.Instance.GetNextNode(currentNode, positions, false).index == entity.CurrentNode.index)
                {
                        Debug.Log("Combat");
                        int damageToTake = entity.baseDamage;

                        entity.TakeDamage(baseDamage);

                        //This means if i'm not dead
                        if (!TakeDamage(damageToTake))
                            Move();

                  
                }
            }
        }
    }
}
