using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationMatchFound : MonoBehaviour
{
    public GameObject buttons;
    public GameObject handsanimator;
    public GameObject handsrotation;
    public AudioSource wardrums;

    bool buttonsactive=true;
    public bool playerwhite;
    public bool testanimation;

//Buttonsdisappear Buttonsreappear Handsreappear

    public void DisappearingButtons()
    {
      if (testanimation) {
        if (buttonsactive) {
          StartCoroutine(FindingMatch());
          wardrums.Play();
        }
      }else{
        buttons.GetComponent<Animator>().Play("Buttonsdisappear");
        buttonsactive=false;
      }

    }

    public void DeleteButtons()
    {

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
      if (playerwhite) {
        //handsrotation.transform.rotation = Quaternion.Euler(0, 90, 0);
        RectTransform rectTransform = handsrotation.GetComponent<RectTransform>();
        //rectTransform.transform.rotation = Quaternion.Euler(0, 0, 180);
        rectTransform.Rotate( new Vector3( 0, 0, 180 ) );
      }
      handsanimator.GetComponent<Animator>().Play("Handsreappear");
      yield return new WaitForSeconds(4);
    }

    IEnumerator FindingMatch()
    {
      buttons.GetComponent<Animator>().Play("Buttonsdisappear");
      buttonsactive=false;
      yield return new WaitForSeconds(2);
      AppearingHands();
    }
}
