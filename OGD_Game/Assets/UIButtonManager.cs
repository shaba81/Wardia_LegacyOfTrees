using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{
    public GameObject unitCards;
    public GameObject buildingsCards;

    void Start()
    {
        buildingsCards.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showUnitCards()
    {
        unitCards.SetActive(true);
        buildingsCards.SetActive(false);
    }

    public void showBuildingCards()
    {
        buildingsCards.SetActive(true);
        unitCards.SetActive(false);
    }
}
