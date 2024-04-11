using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BabyInstructionsEvents : MonoBehaviour
{
    //a few things must happen in this scene: 
    //1. the first set of instructions is present on awake
    //2. a listener (bool) is added for when the baby is successfully picked up
    //3. a listener (bool) is added for when the baby is successfully put down
    //4. when the baby is picked up, it triggers instruction2
    //5. when baby is put down, it triggers instruction3
    //6. there is a continue-on condition that triggers the trial scene (raising the right hand)

    //7. add animation to baby model so that it lifts picked up

    public GameObject instructions1;
    public GameObject instructions2;
    public GameObject instructions3;

    public GameObject GrabbableBabyModel;
    public bool PickedUp = false;
    public bool FirstPickUp = true; 
    public bool PutDown = false;
    public bool FirstPutDown = true;

    public bool Instructions2Over = false;
    public bool Instructions3Over = false;

    public BabyWander babywander;
    public RaiseHand raisehand;

    //[SerializeField] private Animator babyanimationcontrol;

    void Start()
    {
        instructions1.active = true;
        instructions2.active = false;
        instructions3.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool PickedUp = GrabbableBabyModel.GetComponent<TwoHandGrabInteractable>().TwoHandGrab;
        //if the baby is picked up, turn off baby wander script
        if (PickedUp == true)
        {
            babywander.enabled = false;
            //animate to lifted position
            //babyanimationcontrol.SetBool("PickedUp", true);

            //removed on 01.31.24 due to unavoidable overlapping
            if (FirstPickUp == true) //note if this is the first instance of pick up
            {
                //instructions2.active = true; 
                FirstPickUp = false;
                //add a coroutine so that the instructions dont overlap with eachother
                //StartCoroutine("WaitForInstructions2");

            }   
        }
        if (PickedUp == false)
        {
            babywander.enabled = true;
            //babyanimationcontrol.SetBool("PickedUp", false);; //return baby to crawl
            if (FirstPickUp == false && FirstPutDown == true)
            {
                instructions3.active = true;
                FirstPutDown = false;
                StartCoroutine("WaitForInstructions3");
            }
        }
        if (raisehand.Rhigher == true && Instructions3Over == true)
        {
            SceneManager.LoadScene(4);
        }

    }
    IEnumerator WaitForInstructions2()
    {
        yield return new WaitForSeconds(7);
        Instructions2Over = true;
    }
    IEnumerator WaitForInstructions3()
    {
        yield return new WaitForSeconds(8);
        Instructions3Over = true;
    }
}
