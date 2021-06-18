
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TeamManager
{
    private static TeamManager _instance = null;

    protected TeamManager()
    { }
    
    private Team team = Team.None;
        public static TeamManager Instance {
            get {
                if (_instance == null) {
                _instance = new TeamManager();
                }
            return _instance;
            }     
        }

    public void SetTeam(Team team){
        this.team = team;
    }

    
    public Team GetTeam(){
        return team;
    }


}