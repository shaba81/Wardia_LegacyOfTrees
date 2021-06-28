
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICard : MonoBehaviour
{
    public Image icon;
    public new Text name;
    public Image abilityicon;
    public Image abilityicon2;
    public Text cost;
    public Text damage;
    public Text health;
    public bool clickable = true;

    private UIShop shopRef;
    private DeckManager deckmanagerRef;
    private EntitiesDatabaseSO.EntityData myData;

    public void Setup(EntitiesDatabaseSO.EntityData myData, UIShop shopRef)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();
        damage.text = myData.damage.ToString();
        health.text = myData.health.ToString();
        //movement.text = myData.movement.ToString();

        if (myData.name.Equals("Tiger"))
        {
            Sprite nimble = Resources.Load<Sprite>("nimble");
            Sprite runner = Resources.Load<Sprite>("runner");
            abilityicon.GetComponent<Image>().sprite = nimble;
            abilityicon2.GetComponent<Image>().sprite = runner;
        }
        else if (myData.name.Equals("Peasant"))
        {
            Sprite builder = Resources.Load<Sprite>("builder");
            abilityicon.GetComponent<Image>().sprite = builder;
            abilityicon2.enabled = false;
        }
        else if (myData.name.Equals("Avenger"))
        {
            Sprite builder = Resources.Load<Sprite>("builder");
            abilityicon.GetComponent<Image>().sprite = builder;
            abilityicon2.enabled = false;
        }
        else if (myData.movement == 2)
        {
            Sprite runner = Resources.Load<Sprite>("runner");
            abilityicon.GetComponent<Image>().sprite = runner;
            abilityicon2.enabled = false;
        }
        else
        {
            abilityicon.enabled = false;
            abilityicon2.enabled = false;
        }

        this.myData = myData;
        this.shopRef = shopRef;
    }

    public void AltSetup(EntitiesDatabaseSO.EntityData myData, DeckManager deckref)
    {
        icon.sprite = myData.icon;
        name.text = myData.name;
        cost.text = myData.cost.ToString();
        damage.text = myData.damage.ToString();
        health.text = myData.health.ToString();
        //movement.text = myData.movement.ToString();

        if (myData.name.Equals("Tiger")) {
            Sprite nimble = Resources.Load <Sprite>("nimble");
            Sprite runner = Resources.Load <Sprite>("runner");
            abilityicon.GetComponent<Image>().sprite = nimble;
            abilityicon2.GetComponent<Image>().sprite = runner;
        }else if (myData.name.Equals("Peasant")) {
            Sprite builder = Resources.Load <Sprite>("builder");
            abilityicon.GetComponent<Image>().sprite = builder;
            abilityicon2.enabled=false;
        }else if (myData.name.Equals("Avenger")) {
            Sprite builder = Resources.Load <Sprite>("builder");
            abilityicon.GetComponent<Image>().sprite = builder;
            abilityicon2.enabled=false;
        }else if (myData.movement==2) {
            Sprite runner = Resources.Load <Sprite>("runner");
            abilityicon.GetComponent<Image>().sprite = runner;
            abilityicon2.enabled=false;
        }else{
          abilityicon.enabled=false;
          abilityicon2.enabled=false;
        }

        this.myData = myData;
        this.shopRef = null;
        this.deckmanagerRef = deckref;
    }

    public void DeckSetup(EntitiesDatabaseSO.EntityData myData, DeckManager deckref)
    {

    }

    public void OnClick()
    {
        if (clickable)
        {
            if (SceneManager.GetActiveScene().name == "GameScene" && TurnManager.Instance.gameState == GameState.Buying)
            {
                shopRef.OnCardClick(this, myData);
            }
            else if (SceneManager.GetActiveScene().name == "DeckMakingScene") //U can only find cards in the deck-making scene other than in the actual gamescene.
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
