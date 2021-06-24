using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RosterCard : MonoBehaviour
{
    public new Text name;
    public Text cost;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData)
    {
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.myData = myData;
    }

}
