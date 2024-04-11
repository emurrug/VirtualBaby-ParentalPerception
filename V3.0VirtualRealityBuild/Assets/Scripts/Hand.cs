using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{
    Animator animator;
    public float speed;
    private float grabTarget;
    private float grabCurrent;
    private string animatorGrabParam = "Grab";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    internal void SetGrab(float v)
    {
        grabTarget = v;
    }

    void AnimateHand()
    {
        if(grabCurrent != grabTarget)
        {
            grabCurrent = Mathf.MoveTowards(grabCurrent, grabTarget, Time.deltaTime * speed);
            animator.SetFloat(animatorGrabParam, grabCurrent);
        }
    }

}
