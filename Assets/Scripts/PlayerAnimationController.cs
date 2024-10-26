using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Function triggered by the animation event
    public void OnAnimationEnd()
    {
        animator.SetTrigger("GoToIdle");
    }
}
