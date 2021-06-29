using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMatchFound : MonoBehaviour
{
    public GameObject buttons;
    public GameObject handsanimator;
    public GameObject handsrotation;
    public AudioSource wardrums;
        public GameObject rules;
        public GameObject enterName;
            public GameObject inputField;
    bool buttonsactive=true;
    public bool playerwhite;
    public bool testanimation;

//Buttonsdisappear Buttonsreappear Handsreappear

    
    public void MatchFoundCoroutine()
    {
          StartCoroutine(MatchFound());
          wardrums.Play();

    }

    public void DeleteButtons()
    {
        rules.gameObject.SetActive(false);
        enterName.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        buttons.GetComponent<Animator>().Play("Buttonsdisappear");
        buttonsactive=false;

    }

    public void ReappearingButtons()
    {
      if(!buttonsactive)
      {
        buttons.GetComponent<Animator>().Play("Buttonsreappear");
        buttonsactive=true;
      }

    }

    public IEnumerator AppearingHands()
    {
      Debug.Log("starting animation");
      if (playerwhite) {
        //handsrotation.transform.rotation = Quaternion.Euler(0, 90, 0);
        RectTransform rectTransform = handsrotation.GetComponent<RectTransform>();
        //rectTransform.transform.rotation = Quaternion.Euler(0, 0, 180);
        rectTransform.Rotate( new Vector3( 0, 0, 180 ) );
      }
      handsanimator.GetComponent<Animator>().Play("Handsreappear");
      Debug.Log("Finish animation");
      yield return new WaitForSeconds(5);
      LevelLoader.Instance.LoadGameScene();
    }

    IEnumerator MatchFound()
    {
      Debug.Log("MatchFound");
      buttonsactive=false;
      yield return new WaitForSeconds(2);
      StartCoroutine(AppearingHands());
    }
}
