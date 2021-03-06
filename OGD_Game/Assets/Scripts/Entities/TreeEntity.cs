using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntity : MonoBehaviour
{
    public bool isConquered = false;
    private Team conquerer = Team.None;
    public Tile parent;
    public AudioSource conquerSound;

    public GameObject whiteTree;
    public GameObject darkTree;
    public GameObject normalTree;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.myTeam == Team.Team2)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetConquerer(Team team)
    {
        DeActivateConquerSprites();

        if(team == Team.Team1)
        {
            //parent.SetHighlight(true, true);
            whiteTree.gameObject.SetActive(true);

        } else if (team == Team.Team2)
        {
            //parent.SetHighlight(true, false);
            darkTree.gameObject.SetActive(true);
        }
        else
        {
            normalTree.gameObject.SetActive(true);
        }

        conquerSound.Play();
        isConquered = true;
        conquerer = team;
    }

    private void DeActivateConquerSprites()
    {
        normalTree.gameObject.SetActive(false);
        whiteTree.gameObject.SetActive(false);
        darkTree.gameObject.SetActive(false);
    }

    public Team GetConquerer()
    {
        return conquerer;
    }
}
