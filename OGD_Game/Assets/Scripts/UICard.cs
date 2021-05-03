
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICard : MonoBehaviour
{
    public Image icon;
    public new Text name;
    public Text cost;
    public bool clickable = true;

    private UIShop shopRef;
    private DeckManager deckmanagerRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.myData = myData;
        this.shopRef = shopRef;
    }

    public void AltSetup(EntitiesDatabaseSO.EntityData myData, DeckManager deckref)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();

        this.myData = myData;
        this.shopRef = null;
        this.deckmanagerRef = deckref;
    }

    public void OnClick()
    {
        if (clickable)
        {
            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                shopRef.OnCardClick(this, myData);
            }
            else //U can only find cards in the deck-making scene other than in the actual gamescene.
            {
                //If its clicked in the "All Cards Panel"
                //  If Unlocked:
                //      - Ask if u want to add it to the deck
                //          - Add it
                deckmanagerRef.AddCard(this, myData);
            }
        }
        if (!clickable)
        {
            //maybe remove card
        }


    }
}
