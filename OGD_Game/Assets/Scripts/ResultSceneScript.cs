using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResultSceneScript : MonoBehaviour
{
    public Image white;
    public Image black;
    public Text winnerText;
    public Transform arrivalPoint;
    private Team winner = Team.Team1;
    private Team myTeam = Team.Team1;
    public float moveXspeed;

    // Start is called before the first frame update
    void Start()
    {
        winner = GameManager.Instance.GetWinner();
        myTeam = GameManager.Instance.myTeam;
    }

    // Update is called once per frame
    void Update()
    {
        if(winner == Team.Team1)
        {
            if(white.transform.position.x > arrivalPoint.position.x)
            {
                white.transform.position -= new Vector3(moveXspeed, 0) * Time.deltaTime;
            }
            else
            {
                if(winner != myTeam)
                {
                    winnerText.text = "You Lost!";
                    winnerText.gameObject.SetActive(true);
                }
                else if (winner == myTeam)
                {
                    winnerText.text = "You Won!";
                    winnerText.gameObject.SetActive(true);
                }
            }
        }
        else if (winner == Team.Team2)
        {
            if (black.transform.position.x > arrivalPoint.position.x)
            {
                black.transform.position -= new Vector3(moveXspeed, 0) * Time.deltaTime;
            }
            else
            {
                if (winner != myTeam)
                {
                    winnerText.text = "You Lost!";
                    winnerText.gameObject.SetActive(true);
                }
                else if (winner == myTeam)
                {
                    winnerText.text = "You Won!";
                    winnerText.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            winnerText.text = "Draw!";
            winnerText.gameObject.SetActive(true);
        }
    }
}
