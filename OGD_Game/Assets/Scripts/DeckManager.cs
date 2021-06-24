using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public List<RosterCard> unitsCards;
    public List<RosterCard> buildingsCards;
    public List<UICard> allCards;
    public List<GameObject> unitsCursors;
    public List<GameObject> buildingsCursors;

    public EntitiesDatabaseSO cardsDb;
    public EntitiesDatabaseSO unitsDb;
    public EntitiesDatabaseSO buildingsDb;

    public GameObject unitsObject;
    public GameObject buildingsObject;

    public Color buttonSelectedColor;
    public Color buttonUnselectedColor;

    public Image units_button;
    public Image buildings_button;


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

            unitsCards[i].Setup(unitsDb.allEntities[i]);
        }

        for (int i = 0; i < buildingsCards.Count; i++)
        {
            if (!buildingsCards[i].gameObject.activeSelf)
                buildingsCards[i].gameObject.SetActive(true);

            buildingsCards[i].Setup(buildingsDb.allEntities[i]);
        }
        

    }

    public void AddCard(UICard card, EntitiesDatabaseSO.EntityData myData)
    {
        if(!myData.isBuilding)
        {
            if(index < 5)
            {
                unitsCursors[index].SetActive(false);
                unitsDb.allEntities[index] = myData;
                unitsCards[index].gameObject.SetActive(true);
                unitsCards[index].Setup(unitsDb.allEntities[index]);

                index++;
                if(index != 5)
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
                buildingsCards[buildingsIndex].Setup(buildingsDb.allEntities[buildingsIndex]);

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
        if (index != 5)
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
    public void SwitchToBuildingsDeck()
    {
        units_button.color = buttonUnselectedColor;
            unitsObject.SetActive(false);
        buildings_button.color = buttonSelectedColor;
            buildingsObject.SetActive(true);
    }

    public void SwitchToUnitsDeck()
    {
        buildings_button.color = buttonUnselectedColor;
            buildingsObject.SetActive(false);
        units_button.color = buttonSelectedColor;
            unitsObject.SetActive(true);
    }

}
