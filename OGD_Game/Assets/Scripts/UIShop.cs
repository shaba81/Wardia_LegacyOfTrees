using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    public List<UICard> unitsCards;
    public List<UICard> buildingCards;
    public Text money;

    private EntitiesDatabaseSO cachedDb;
    private int refreshCost = 1;

    private void Start()
    {
        cachedDb = GameManager.Instance.entitiesDatabase;
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

            unitsCards[i].Setup(cachedDb.allEntities[i], this);
        }
    }

    public void OnCardClick(UICard card, EntitiesDatabaseSO.EntityData cardData)
    {
        //We should check if we have the money!
        if (PlayerData.Instance.CanAfford(cardData.cost))
        {
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
