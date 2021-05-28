using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public List<UICard> unitsCards;
    public List<UICard> buildingsCards;
    public List<UICard> allCards;
    public List<GameObject> unitsCursors;
    public List<GameObject> buildingsCursors;

    public EntitiesDatabaseSO cardsDb;
    public EntitiesDatabaseSO unitsDb;
    public EntitiesDatabaseSO buildingsDb;

    public GameObject unitsObject;
    public GameObject buildingsObject;


    private 

    int index = 0;
    int buildingsIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        buildingsIndex = 0;
        LoadCards();
    }

    private void LoadCards()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (!allCards[i].gameObject.activeSelf)
                allCards[i].gameObject.SetActive(true);

            allCards[i].AltSetup(cardsDb.allEntities[i], this);
        }

        for (int i = 0; i < unitsCards.Count; i++)
        {
            if (!unitsCards[i].gameObject.activeSelf)
                unitsCards[i].gameObject.SetActive(true);

            unitsCards[i].AltSetup(unitsDb.allEntities[i], this);
            unitsCards[i].clickable = false;
        }

        for (int i = 0; i < buildingsCards.Count; i++)
        {
            if (!buildingsCards[i].gameObject.activeSelf)
                buildingsCards[i].gameObject.SetActive(true);

            buildingsCards[i].AltSetup(buildingsDb.allEntities[i], this);
            buildingsCards[i].clickable = false;
        }

    }

    public void AddCard(UICard card, EntitiesDatabaseSO.EntityData myData)
    {
        if(!myData.isBuilding)
        {
            if(index < 4)
            {
                unitsCursors[index].SetActive(false);
                unitsDb.allEntities[index] = myData;
                unitsCards[index].gameObject.SetActive(true);
                unitsCards[index].AltSetup(unitsDb.allEntities[index], this);
                unitsCards[index].clickable = false;

                index++;
                if(index != 4)
                {
                    unitsCursors[index].SetActive(true);
                } 
            }
        } 
        else if (myData.isBuilding)
        {
            Debug.Log("Building clicked");
            if (buildingsIndex < 3)
            {
                buildingsCursors[buildingsIndex].SetActive(false);
                buildingsDb.allEntities[buildingsIndex] = myData;
                buildingsCards[buildingsIndex].gameObject.SetActive(true);
                buildingsCards[buildingsIndex].AltSetup(buildingsDb.allEntities[buildingsIndex], this);
                buildingsCards[buildingsIndex].clickable = false;

                buildingsIndex++;
                if (buildingsIndex != 3)
                {
                    buildingsCursors[buildingsIndex].SetActive(true);
                }
            }
        }
    }

    public void RemoveCard()
    {

    }

    public void SaveDeck()
    {
        //send the updated deckDb in network
    }

    public void ResetDeck()
    {
        if (index != 4)
        {
            unitsCursors[index].SetActive(false);  
        }
        index = 0;
        unitsCursors[index].SetActive(true);

        if (buildingsIndex != 4)
        {
            buildingsCursors[index].SetActive(false);
        }
        buildingsIndex = 0;
        buildingsCursors[index].SetActive(true);


        //in deck database initialize the default deck.
    }

    //Used to switch from units and buildings.
    public void SwitchDeck()
    {
        if(unitsObject.active)
        {
            unitsObject.SetActive(false);
            buildingsObject.SetActive(true);
        }
        else if (!unitsObject.active)
        {
            buildingsObject.SetActive(false);
            unitsObject.SetActive(true);
        }
    }

}
