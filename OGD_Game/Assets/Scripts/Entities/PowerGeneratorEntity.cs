using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGeneratorEntity : BaseEntity
{
    private int turnCounter = 0;

    protected override void OnRoundStart()
    {
        if(this.myTeam == GameManager.Instance.myTeam)
        {
            turnCounter++;
            if(turnCounter == 2)
            {
                PlayerData.Instance.GiveMoney(5);
                PopUpManager.Instance.SpawnPopUp(this.transform.position, PopUpType.FivePoints);
                UITreeUpdater.Instance.UpdateTrees();
                Debug.Log("Entity: " + name + " gave 5 points.");
                TakeDamage(1);
            }
        }

    }

    public override void OnRoundEnd()
    {

    }
}
