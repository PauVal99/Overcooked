using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Run(){
        animator.SetBool("Run", true);
    }

    public void Idle(){
        animator.SetBool("Run", false);
    }

    public void Carry(){
        animator.SetBool("Carry", true);
    }

    public void StopCarry(){
        animator.SetBool("Carry", false);
    }

    public void Cut(){
        animator.SetBool("Cut", true);
    }

    public void StopCutting(){
        animator.SetBool("Cut", false);
    }

}
