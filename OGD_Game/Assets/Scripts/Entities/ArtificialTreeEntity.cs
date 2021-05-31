using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialTreeEntity : BaseEntity
{
    //Gives 1 nature point every start of your turn
    protected override void OnRoundStart()
    {
        PlayerData.Instance.GiveMoney(1);
        Debug.Log("Artificial Tree Gave 1 point.");
    }
}
