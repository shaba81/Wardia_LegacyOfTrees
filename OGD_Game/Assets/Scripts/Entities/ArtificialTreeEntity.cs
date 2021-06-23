using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialTreeEntity : BaseEntity
{
    //Gives 1 nature point every start of your turn
    protected override void OnRoundStart()
    {
        if (this.myTeam == GameManager.Instance.myTeam)
        {
            PlayerData.Instance.GiveMoney(1);
            PopUpManager.Instance.SpawnPopUp(this.transform.position, PopUpType.OnePoint);
            Debug.Log("Artificial Tree Gave 1 point.");

        }
    }
}
