using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFxManager : Manager<SoundFxManager>
{
    public AudioClip[] audioClips;
    public AudioSource source;

    public void PlaySpawn()
    {
        source.clip = audioClips[0];
        source.Play();
    }

    public void PlayBuilding()
    {
        source.clip = audioClips[1];
        source.Play();
    }

    public void PlayBuilder()
    {
        source.clip = audioClips[2];
        source.Play();
    }

    public void PlayEvangelist()
    {
        source.clip = audioClips[3];
        source.Play();
    }

    public void PlayAvenger()
    {
        source.clip = audioClips[4];
        source.Play();
    }

    public void PlayTiger()
    {
        source.clip = audioClips[5];
        source.Play();
    }

    public void PlayJaguar()
    {
        source.clip = audioClips[6];
        source.Play();
    }
}
