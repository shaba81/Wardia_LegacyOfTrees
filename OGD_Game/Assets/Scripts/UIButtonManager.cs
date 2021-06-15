using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIButtonManager : Manager<UIButtonManager>
{
    public GameObject unitCards;
    public GameObject buildingsCards;

    public Image fullUnitImage;
    public Image emptyUnitImage;

    public Image fullBuildingImage;
    public Image emptyBuildingImage;

    public Button changePhaseButton;

    void Start()
    {
        buildingsCards.SetActive(false);

        fullBuildingImage.enabled = false;
        emptyBuildingImage.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(TurnManager.Instance.gameState == GameState.Buying)
        {
            changePhaseButton.interactable = true;
        } else 
        {
            changePhaseButton.interactable = false;
        }
    }

    public void showUnitCards()
    {
        fullUnitImage.enabled = true;
        emptyUnitImage.enabled = false;

        unitCards.SetActive(true);
        buildingsCards.SetActive(false);

        fullBuildingImage.enabled = false;
        emptyBuildingImage.enabled = true;

    }

    public void showBuildingCards()
    {
        fullBuildingImage.enabled = true;
        emptyBuildingImage.enabled = false;

        buildingsCards.SetActive(true);
        unitCards.SetActive(false);

        fullUnitImage.enabled = false;
        emptyUnitImage.enabled = true;
    }

    public void DisableEndButton()
    {
        changePhaseButton.interactable = false;
    }

    public void EnableEndButton()
    {
        changePhaseButton.interactable = true;
    }
}
