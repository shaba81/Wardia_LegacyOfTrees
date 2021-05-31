using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneratorEntity : BaseEntity
{
    private int turnCounter = 0;

    protected override void OnRoundStart()
    {
        turnCounter++;
        if(turnCounter == 2)
        {
            PlayerData.Instance.GiveMoney(5);
            UITreeUpdater.Instance.UpdateTrees();
            Debug.Log("Entity: " + name + " gave 5 points.");
            TakeDamage(1);
        }

    }

    protected override void OnRoundEnd()
    {

    }
}
