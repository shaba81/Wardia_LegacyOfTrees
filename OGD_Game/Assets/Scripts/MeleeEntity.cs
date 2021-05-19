using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntity : BaseEntity
{
    protected override void OnRoundStart()
    {
        //FindTarget();
        Debug.Log("Action on round start");

    }

    public void Update()
    {
        
        if (!HasEnemy)
        {
            FindTarget();
        }

        if (IsInRange && !moving)
        {
            //In range for attack!
            if (canAttack)
            {
                Attack();
                currentTarget.TakeDamage(baseDamage);
            }
        }
        else
        {
            GetInRange();
        }
    }
}
