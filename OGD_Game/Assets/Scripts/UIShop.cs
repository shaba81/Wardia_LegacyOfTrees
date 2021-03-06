using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public List<UICard> unitsCards;
    public List<UICard> buildingCards;
    public Text money;

    private EntitiesDatabaseSO unitsDb;
    private EntitiesDatabaseSO buildingsdDb;
    public EntitiesDatabaseSO allCardsDb;

    private List<string> unitsNames;
    private List<string> buildingsNames;

    private int refreshCost = 1;

    private void Start()
    {
        unitsDb = GameManager.Instance.entitiesDatabase;
        buildingsdDb = GameManager.Instance.buildingsDatabase;

        unitsNames = PlayerPrefsX.GetStringArray("Units").ToList();
        buildingsNames = PlayerPrefsX.GetStringArray("Buildings").ToList();

        GenerateCard();
        PlayerData.Instance.OnUpdate += Refresh;
        Refresh();
    }

    public void GenerateCard()
    {
        for (int i = 0; i < unitsCards.Count; i++)
        {
            if (!unitsCards[i].gameObject.activeSelf)
                unitsCards[i].gameObject.SetActive(true);

            if(unitsNames == null)
            {
                if (!unitsDb.allEntities[i].isBuilding)
                    unitsCards[i].Setup(unitsDb.allEntities[i], this);

            }
            else
            {
                foreach (EntitiesDatabaseSO.EntityData _data in allCardsDb.allEntities)
                {
                    if (_data.name.Equals(unitsNames[i]))
                    {
                        unitsCards[i].Setup(_data, this);
                    }
                }

            }
        }

        for (int i = 0; i < buildingCards.Count; i++)
        {
            if (!buildingCards[i].gameObject.activeSelf)
                buildingCards[i].gameObject.SetActive(true);

            if(buildingsNames == null)
            {
                if(buildingsdDb.allEntities[i].isBuilding)
                    buildingCards[i].Setup(buildingsdDb.allEntities[i], this);
                
            }
            else
            {
                foreach (EntitiesDatabaseSO.EntityData _data in allCardsDb.allEntities)
                {
                    if (_data.name.Equals(buildingsNames[i]))
                    {
                        buildingCards[i].Setup(_data, this);
                    }
                }

            }
        }
    }

    public void OnCardClick(UICard card, EntitiesDatabaseSO.EntityData cardData)
    {
        //We should check if we have the money!
        if (PlayerData.Instance.CanAfford(cardData.cost))
        {
            if(cardData.name.Equals("Avatar"))
            {

            }
            //We should check if we can actually place that card, some cards have requirements other than cost.
            if (!GameManager.Instance.checkTreeRequirement(cardData.treeRequirement))
            {
                Debug.Log("You need at least " + cardData.treeRequirement + " trees conquered!");
                return;
            }

            //check if we have builders
            if(cardData.isBuilding)
            {
                if(!GameManager.Instance.CheckBuilders(GameManager.Instance.myTeam))
                {
                    Debug.Log("You need at least 1 builder in your board!");
                    return;
                }
            }

            PlayerData.Instance.SpendMoney(cardData.cost);
            //card.gameObject.SetActive(false);
            GameManager.Instance.OnEntityBought(cardData);
        }
    }

    public void OnRefreshClick()
    {
        //Decrease money 
        if (PlayerData.Instance.CanAfford(refreshCost))
        {
            PlayerData.Instance.SpendMoney(refreshCost);
            GenerateCard();
        }
    }

    void Refresh()
    {
        money.text = PlayerData.Instance.Money.ToString();
    }
}
