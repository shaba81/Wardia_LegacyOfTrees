using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : Manager<PopUpManager>
{
    public Transform attackPopUp;
    public Transform defensePopUp;
    public Transform onePointPopUp;
    public Transform fivePointsPopUp;

    public void SpawnPopUp(Vector3 position, PopUpType type)
    {
        switch (type)
        {
            case PopUpType.Health:
                Instantiate(defensePopUp, position, Quaternion.identity);
                break;
            case PopUpType.Damage:
                Instantiate(attackPopUp, position, Quaternion.identity);
                break;
            case PopUpType.OnePoint:
                Instantiate(onePointPopUp, position, Quaternion.identity);
                break;
            case PopUpType.FivePoints:
                Instantiate(fivePointsPopUp, position, Quaternion.identity);
                break;
            default:
                break;
        }
    }
}

public enum PopUpType
{
    Health,
    Damage,
    OnePoint,
    FivePoints
}
