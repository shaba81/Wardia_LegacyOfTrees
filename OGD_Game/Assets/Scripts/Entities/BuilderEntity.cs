using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderEntity : BaseEntity
{

    protected override void OnRoundStart()
    {
        
        GameManager.Instance.IncreaseBuilderCounter(myTeam);
        GameManager.Instance.OnRoundStart -= OnRoundStart;

    }

    protected override void OnRoundEnd()
    {
        
        if (isFirstTurn)
        {           
            return;
        }

        if (!Move())
        {
            //means there's someone on the other tile.
            //if it's an enemy, combat;
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                if (GridManager.Instance.GetNextNode(currentNode) == entity.CurrentNode)
                {
                    Debug.Log("Combat");
                }
                //if it's an entity from Team1 just do nothing;
            }
        }
        Debug.Log("Action Ended from entity: " + this);

    }

    protected override void OnUnitDied(BaseEntity diedUnity)
    {
        GameManager.Instance.DecreaseBuilderCounter(myTeam);
    }





}