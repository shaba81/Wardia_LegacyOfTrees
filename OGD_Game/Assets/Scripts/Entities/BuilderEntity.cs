using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderEntity : BaseEntity
{
    protected override void AddPlacingConditions()
    {
        if(name.Contains("Peasant"))
        {
            foreach(BaseEntity entity in GameManager.Instance.GetMyEntities(myTeam))
            {
                if (entity.name.Contains("Evangelist"))
                {
                    foreach (Node node in GridManager.Instance.Neighbors(entity.CurrentNode))
                    {
                        if(!node.IsOccupied)
                            eligibleNodes.Add(node);
                    }
                }
            }
        }
    }

    protected override void OnRoundStart()
    {

        isBuilder = true;
        GameManager.Instance.IncreaseBuilderCounter(myTeam);
        GameManager.Instance.OnRoundStart -= OnRoundStart;

    }


    public override void OnRoundEnd()
    {

        if (!Move())
        {
            //means there's someone on the other tile.
            //if it's an enemy, combat;
            Debug.Log("CANT MOVE");
            foreach (BaseEntity entity in GameManager.Instance.GetEntitiesAgainst(myTeam))
            {
                Debug.Log(entity.name);
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

    protected override void OnUnitDied(BaseEntity diedUnity)
    {
        GameManager.Instance.DecreaseBuilderCounter(myTeam);
    }





}
