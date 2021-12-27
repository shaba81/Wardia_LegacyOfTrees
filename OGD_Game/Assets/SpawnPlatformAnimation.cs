using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatformAnimation : MonoBehaviour
{
    Animator anim;
    ParticleSystem fx;

    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponent<Animator>();
        fx = GetComponentInChildren<ParticleSystem>();
    }

    public void Spawn()
    {
        fx.Play();
        anim.SetBool("Spawned", true);
    }

    public void Despawn()
    {
        anim.SetBool("Spawned", false);
    }
}
