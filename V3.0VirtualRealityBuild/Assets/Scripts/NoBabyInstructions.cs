using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoBabyInstructions : MonoBehaviour
{
    //the first set of instructions will play automatically
    //participants will be able to freely move their hands, but whenever their hand goes up it dings
    //if their hand goes up during an approprite time, then their score counter goes up (can reserve this for the actual trials)
    //instrctions wait for hand wave to continue
    //new sound bite trigger
    //wait for them to raise their hand
    //continue to next scene

    public bool InstructionsComplete = false;
    public bool Instructions2Complete = false;
    public RaiseHand raisehand;
    public CarPath carpath;

    public GameObject instructions1;
    public GameObject instructions2;

    void Start()
    {
        StartCoroutine("WaitForInstructions");
        instructions1.active = true;
        instructions2.active = false;
    }

    void Update()
    {
        if (carpath.AlreadyScored == true &&
            InstructionsComplete == true)
        {
            instructions2.active = true;
            StartCoroutine("WaitForInstructions2");
        }

        if (raisehand.Rhigher == true && Instructions2Complete == true)
        {
            SceneManager.LoadScene(2);
        }
        if (raisehand.Lhigher == true && Instructions2Complete == true)
        {
            SceneManager.LoadScene(1);
        }
    }

    
    IEnumerator WaitForInstructions()
    {
        yield return new WaitForSeconds(47);
        InstructionsComplete = true;
    }
    IEnumerator WaitForInstructions2()
    {
        yield return new WaitForSeconds(20);
        Instructions2Complete = true;
    }
}
