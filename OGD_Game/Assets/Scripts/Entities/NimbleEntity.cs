using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NimbleEntity : BaseEntity
{
    public override void OnRoundEnd()
    {

        if (!Move(movement))
        {
            //means there's someone on the other tile.
            //if it's an enemy, combat;
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                
                if (GridManager.Instance.GetNextNode(currentNode, positions, true, myTeam).index == entity.CurrentNode.index)
                {
                        Debug.Log("Combat");
                        int damageToTake = entity.baseDamage;

                        if (!entity.TakeDamage(baseDamage))
                        {
                            TakeDamage(damageToTake);
                        }

                        if (!dead)
                            Move(2);

                
                }
            }
        }
    }

}
