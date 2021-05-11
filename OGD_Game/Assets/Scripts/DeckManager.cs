using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{

    public List<UICard> deckCards;
    public List<UICard> allCards;
    public List<GameObject> cursors;

    public EntitiesDatabaseSO cardsDb;
    public EntitiesDatabaseSO deckDb;

    private 

    int index = 0;


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
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

        for (int i = 0; i < deckCards.Count; i++)
        {
            if (!deckCards[i].gameObject.activeSelf)
                deckCards[i].gameObject.SetActive(true);

            deckCards[i].AltSetup(deckDb.allEntities[i], this);
            deckCards[i].clickable = false;
        }

    }

    public void AddCard(UICard card, EntitiesDatabaseSO.EntityData myData)
    {
        if(index < 4)
        {
            cursors[index].SetActive(false);
            Debug.Log(index);
            deckDb.allEntities[index] = myData;
            deckCards[index].gameObject.SetActive(true);
            deckCards[index].AltSetup(deckDb.allEntities[index], this);
            deckCards[index].clickable = false;

            index++;
            if(index != 4)
            {
                cursors[index].SetActive(true);
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
            cursors[index].SetActive(false);  
        }
        index = 0;
        cursors[index].SetActive(true);


        //in deck database initialize the default deck.
    }

}
