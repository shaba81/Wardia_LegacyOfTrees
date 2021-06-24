using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
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
      //page.Play();
    }

    public void LoadMatchmaking()
    {
      StartCoroutine(LoadLevel("MatchmakingScene"));
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

    IEnumerator LoadLevel(string Levelname)
    {
      transition.SetTrigger("Start");
      yield return new WaitForSeconds(1);
      SceneManager.LoadScene(Levelname);
    }
}
