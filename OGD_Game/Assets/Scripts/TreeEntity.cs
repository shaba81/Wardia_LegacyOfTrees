using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntity : MonoBehaviour
{
    public bool isConquered = false;
    private Team conquerer;
    public Tile parent;

    // Start is called before the first frame update
    void Start()
    {
        conquerer = Team.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetConquerer(Team team)
    {
        if(team == Team.Team1)
        {
            parent.SetHighlight(true, true);

        } else if (team == Team.Team2)
        {
            parent.SetHighlight(true, false);
        }

        isConquered = true;
        conquerer = team;
    }

    public Team GetConquerer()
    {
        return conquerer;
    }
}
