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
      StartCoroutine(LoadLevel(0));
      page.Play();
    }

    public void LoadDeckmaking()
    {
      StartCoroutine(LoadLevel(6));
      page.Play();
    }

    public void LoadMatch()
    {
      StartCoroutine(LoadLevel(1));
      page.Play();
    }

    public void LoadMatchmaking()
    {
      StartCoroutine(LoadLevel(5));
      page.Play();
    }

    public void LoadOption()
    {
      StartCoroutine(LoadLevel(2));
      page.Play();
    }

    public void LoadShop()
    {
      StartCoroutine(LoadLevel(3));
      page.Play();
    }

    public void LoadResultscreen()
    {
      StartCoroutine(LoadLevel(4));
      page.Play();
    }

    IEnumerator LoadLevel(int Levelindex)
    {
      transition.SetTrigger("Start");
      yield return new WaitForSeconds(1);
      SceneManager.LoadScene(Levelindex);
    }
}
