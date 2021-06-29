using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LevelLoader  : Manager<LevelLoader>
{
    public AudioSource page;
    public Animator transition;

    public void LoadMainmenu()
    {
      StartCoroutine(LoadLevel("MainMenu"));
      page.Play();
    }

    public void LoadDeckmaking()
    {
      StartCoroutine(LoadLevel("DeckMakingScene"));
      page.Play();
    }

    public void LoadMatch()
    {
        StartCoroutine(LoadLevel("GameScene"));
        page.Play();
    }

    public void LoadMatchmaking()
    {
      StartCoroutine(LoadLevel("Matchmaking"));
      page.Play();
    }

    public void LoadOption()
    {
      StartCoroutine(LoadLevel("Options"));
      page.Play();
    }

    public void LoadShop()
    {
      StartCoroutine(LoadLevel("Shop"));
      page.Play();
    }

    public void LoadResultscreen()
    {
      StartCoroutine(LoadLevel("ResultScreen"));
      page.Play();
    }
    public void LoadGameScene()
    {
      StartCoroutine(LoadLevel("GameScene"));
      page.Play();
    }

    IEnumerator LoadLevel(string Levelname)
    {
      transition.SetTrigger("Start");
      yield return new WaitForSeconds(1);
      SceneManager.LoadScene(Levelname);
    }
}
