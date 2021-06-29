using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{

    public void Mute()
    {
        AudioListener.volume = 0f;
    }

    public void UnMute()
    {
        AudioListener.volume = 1f;
    }
}
