using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimator : MonoBehaviour
{
    public Animator animator;
    public RuntimeAnimatorController front;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    public void SwitchFront()
    {
        animator.runtimeAnimatorController = front;
    }
}
