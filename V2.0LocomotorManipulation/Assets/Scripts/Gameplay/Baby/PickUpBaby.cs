﻿//this script is for coordinating the animation to pick up the baby
//this script is best understood in conjunction with the animator navigation attached to the "BabyModel" object
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class PickUpBaby : MonoBehaviour
{
    //Baby variables
    NavMeshAgent theBaby; //the baby has a Navigation AI to help it pathfind around objects
    public GameObject BabyNonNavMesh; // the NavMesh can't be picked off the ground, so we need to call the object shell

    public GameObject theDestination; //where the baby wanders to
    public GameObject ReturnPoint; //where the baby is put back to when the player picks them up

    [SerializeField] private Animator babyanimationcontrol; //how to trigger baby animations from the animator

    //Player variables
    public RigidbodyFirstPersonController theParent; //the cylindrical game object with the camera attached
    public GameObject theParentFile; //the empty parent object that includes the FPC, camera, and hands
    public Camera playercamera; //the main camera (attached like a head to FPC)

    public GameObject playerhands; //a set distance in front of the camera (where baby is lifted to) attached to FPC
    public GameObject parentPOV; //a set distance above/behind baby where the camera moves towards for pickup animation
    public GameObject parentPOVArms; //a set distance above baby where parents hands should move to for pickup animation
    public GameObject parentReturnPoint; //the point on the chair where the FPC should return to
    public GameObject parentHandReturnPoint; //the set distance in front of the chair where player hands should return to

    //how to reference the baby mobility chooser script
    //how to reference the baby mobility chooser script
    public string mobility;
    public string clothes;
    public BabyScene babyscene;

    //the bool that triggers the whole animation
    public bool PickBabyUp = false;

    //this standard asset script is referenced to force camera positioning during pick up animation
    public MouseLook mouselookscript;


    void Start()
    {
        theBaby = GetComponent<NavMeshAgent>();
        mouselookscript = theParent.mouseLook;
        playercamera = theParent.GetComponentInChildren<Camera>();
        mouselookscript.XSensitivity = 2f;
        mouselookscript.YSensitivity = 2f;
        mobility = babyscene.MOBILITY;
        clothes = babyscene.CLOTHES;


        if (mobility == "walker")
        {
            babyanimationcontrol.SetInteger("MobilityType", 1);
        }
        else if (mobility == "crawler")
        {
            babyanimationcontrol.SetInteger("MobilityType", 2);
        }
        else if (mobility == "precrawler")
        {
            babyanimationcontrol.SetInteger("MobilityType", 3);
            theBaby.transform.position = ReturnPoint.transform.position;
        }
    }

    void Update()
    {

        //the cue to transition from the "mobilitchooser" default state is set within the Animator panel ("mobilitytype" integer conditions)
        //below only codes for information about parent animation and baby/parent movement. they dont really code for animation cycle
        //the cycle is encoded in the transition elements in the Animator panels. all 3 animation cycles should mirror each other


        //B is the input to trigger pick up baby animation
        if (Input.GetKeyDown(KeyCode.B))
        {
            PickBabyUp = true;
        }


        //if the player didn't hit B, the baby is just moving randomly (See "BabyWander" script)
        if (PickBabyUp == false && 
            mobility != "precrawler" &&
            babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("MobilityChooser") == false)
        {
            theBaby.SetDestination(theDestination.transform.position);
        }
        if (PickBabyUp == true)
        {
            //locks the camera on baby and prevents camera from moving around
            playercamera.transform.forward = (theBaby.transform.position) - (theParent.transform.position);
            mouselookscript.XSensitivity = 0f;
            mouselookscript.YSensitivity = 0f;
            //stops the baby from moving towards goal
            theDestination.transform.position = theBaby.transform.position;

            //checks to make sure baby is in right starting animation
            if (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("CrawlingAnimation") |
                babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("PrecrawlerAnimation") |
                babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("WalkerAnimation"))
            {
                //triggers Sit and Wait animations
                babyanimationcontrol.SetInteger("IsPickedUpTrigger", 1);
                //turning off the pathfinding to allow the baby to be moved off the ground
                theBaby.GetComponent<NavMeshAgent>().enabled = false;
                //babyanimationcontrol.SetBool("LiftOff", true); //bool prevents animation sequence from accidentally looping
            }

    
            if (theParent.transform.position != parentPOV.transform.position)// &&
                //babyanimationcontrol.GetBool("LiftOff") == true)
            {
                if (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("SittingPosition") |
                    babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("PrecrawlerAnimation"))
                { //moves the parent towards baby
                    theParent.transform.position = Vector3.MoveTowards(
                    theParent.transform.position, parentPOV.transform.position, Time.deltaTime * 4);
                    playerhands.transform.position = Vector3.MoveTowards(
                    playerhands.transform.position, parentPOVArms.transform.position, Time.deltaTime * 4);
                }
            }
            if (theParent.transform.position == parentPOV.transform.position &&
                    //babyanimationcontrol.GetBool("LiftOff") == true &&
                    babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("LiftedPosition") == false)
            {
                    babyanimationcontrol.SetInteger("IsPickedUpTrigger", 2);
            }

            if (BabyNonNavMesh.transform.position != playerhands.transform.position &&
                babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("LiftedPosition"))
            {
                //moves baby to player hands
                BabyNonNavMesh.transform.position = Vector3.MoveTowards(
                BabyNonNavMesh.transform.position, playerhands.transform.position, Time.deltaTime * 3);
            }

            if (BabyNonNavMesh.transform.position == playerhands.transform.position &&
                babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("LiftedPosition"))
            {
                //moves the parent backwards to the chair
                theParent.transform.position = Vector3.MoveTowards(
                    theParent.transform.position, parentReturnPoint.transform.position, Time.deltaTime * 3);
                playerhands.transform.position = Vector3.MoveTowards(
                    playerhands.transform.position, parentHandReturnPoint.transform.position, Time.deltaTime * 3);

                //make sure the baby stays on the players hands as the player moves back
                BabyNonNavMesh.transform.position = playerhands.transform.position;
            }

            if (theParent.transform.position == parentReturnPoint.transform.position &&
                babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("LiftedPosition"))
            {
                //triggers Is Set Down animations
                babyanimationcontrol.SetInteger("IsPickedUpTrigger", 3);

                //babyanimationcontrol.SetBool("LiftOff", false);
            }

            if (//babyanimationcontrol.GetBool("LiftOff") == false &&
                BabyNonNavMesh.transform.position != ReturnPoint.transform.position)
            {
                //returns baby to return point on the ground
                BabyNonNavMesh.transform.position = Vector3.MoveTowards(
                BabyNonNavMesh.transform.position, ReturnPoint.transform.position, Time.deltaTime * 1);
            }

            StartCoroutine(WaitAndEnd());
            //starts the script that is looping until baby has reached the final return point


        }


    }
        IEnumerator WaitAndEnd()
        {
            yield return new WaitForSeconds(5); //every 5 seconds, sample if the animation cycle is complete

            if (theBaby.transform.position == ReturnPoint.transform.position &&
            babyanimationcontrol.GetInteger("IsPickedUpTrigger") == 3)
            {
                //baby resumes mobility like normal
                babyanimationcontrol.SetInteger("IsPickedUpTrigger", 4);
                theBaby.GetComponent<NavMeshAgent>().enabled = true;

                //camera returns to normal
                mouselookscript.XSensitivity = 2f;
                mouselookscript.YSensitivity = 2f;

                //allows the animation to be triggered again
                PickBabyUp = false;

            //see "Dialogue2" for reference to why this is set here
            Dialogue2.Advances = 5;

                yield return new WaitForSeconds(7); //time for longest animation to restart (i.e. sitting>standing)
                 //return theDestination object to random movement patterns
                int xPos = Random.Range(249, 262);
                int zPos = Random.Range(178, 195);
                theDestination.transform.position = new Vector3(xPos, 1, zPos);
            }

            //if the animation cycle is not complete, try again in 5 seconds
            else if (theBaby.transform.position != ReturnPoint.transform.position)
            {
                StartCoroutine(WaitAndEnd());
            }




        }
    }

