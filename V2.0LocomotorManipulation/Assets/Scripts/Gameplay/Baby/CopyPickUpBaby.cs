//this script is for coordinating the animation to pick up the baby
//this script is best understood in conjunction with the animator navigation attached to the "BabyModel" object
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class CopyPickUpBaby : MonoBehaviour
{
    //Baby variables
    public NavMeshAgent theBaby; //the baby has a Navigation AI to help it pathfind around objects
    public GameObject BabyNonNavMesh; // the NavMesh can't be picked off the ground, so we need to call the object shell
    //(note: it's easier if the object is set to "TheBabyModel" group (includes the navmesh) rather than "the baby"

    public GameObject theDestination; //where the baby wanders to
    public GameObject ReturnPoint; //where the baby is put back to when the player picks them up

    [SerializeField] private Animator babyanimationcontrol; //how to trigger baby animations from the animator

    //Player variables
    public RigidbodyFirstPersonController theParent; //the cylindrical game object with the camera attached
    public GameObject theParentFile; //the empty parent object that includes the FPC, camera, and hands
    public Camera playercamera; //the main camera (attached like a head to FPC)
    public float theBabyYRot; //will get called to slightly modify the rotation of the player camera (to look at the baby's head instead of butt)
    public float theBabyZRot;
    public float theBabyXRot;

    public GameObject playerhands; //a set distance in front of the camera (where baby is lifted to) attached to FPC
    public GameObject parentPOV; //a set distance above/behind baby where the camera moves towards for pickup animation
    public GameObject parentPOVArms; //a set distance above baby where parents hands should move to for pickup animation
    public GameObject parentReturnPoint; //the point on the chair where the FPC should return to
    public GameObject parentHandReturnPoint; //the set distance in front of the chair where player hands should return to

    //how to reference the baby mobility chooser script
    public string mobility;
    public babymobility babymobility;
    public Texture clothes;

    //this standard asset script is referenced to force camera positioning during pick up animation
    public MouseLook mouselookscript;

    void Start()
    {
        theBaby = GetComponent<NavMeshAgent>();
        mouselookscript = theParent.mouseLook;
        playercamera = theParent.GetComponentInChildren<Camera>();
        mouselookscript.XSensitivity = 2f;
        mouselookscript.YSensitivity = 2f;

        mobility = StaticMobility.MOBILITY;
        clothes = StaticMobility.CLOTHES;

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
            theDestination.transform.position = theBaby.transform.position;
        }

        babyanimationcontrol.SetInteger("StepTrigger", 0);


    }

    //the cue to transition from the "mobilitchooser" default state is set within the Animator panel ("mobilitytype" integer conditions)
    //below only codes for information about parent animation and baby/parent movement. they dont really code for animation cycle
    //the cycle is encoded in the transition elements in the Animator panels. all 3 animation cycles should mirror each other
    void Update()
    {
        //if the player didn't hit B, the baby is just moving randomly (See "BabyWander" script)
        if (babyanimationcontrol.GetInteger("StepTrigger") == 0 && //if the baby isn't being picked up
            mobility != "precrawler" && //and isn't a stationary infant
            babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("MobilityChooser") == false &&
            babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("ReturnToWalking") == false &&
            babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("ReturnToCrawlAnimation") == false) 
        {
            theBaby.SetDestination(theDestination.transform.position);
        }

        //B is the input to trigger pick up baby animation
        if (Input.GetKeyDown(KeyCode.Mouse1) && babyanimationcontrol.GetInteger("StepTrigger") == 0)
        {
            theBaby.GetComponent<NavMeshAgent>().isStopped = true; //had to add this thanks to deprecated function
            babyanimationcontrol.SetInteger("StepTrigger", 2);
            StartCoroutine("MoveOverThere");
        }

        if (babyanimationcontrol.GetInteger("StepTrigger") != 0)
        {
            theBabyYRot = theBaby.GetComponent<Transform>().position.y + 2;
            theBabyZRot = theBaby.GetComponent<Transform>().position.z;
            theBabyXRot = theBaby.GetComponent<Transform>().position.x;

            //locks the parents gaze during animation cycle
            playercamera.transform.LookAt(new Vector3(theBabyXRot, theBabyYRot, theBabyZRot));
            //playercamera.transform.LookAt(theBaby.transform.position);
            mouselookscript.XSensitivity = 0f;
            mouselookscript.YSensitivity = 0f;
            //stops the baby from moving towards goal
            theDestination.transform.position = theBaby.transform.position;
        }

    }

    public void LoadTheAnimation()
    { 
        //removed step 1 in the trigger sequence because of redundancy
        
        if (babyanimationcontrol.GetInteger("StepTrigger") == 3) //GotTheGoods
        {
            StartCoroutine("GetInMyHands");
            Debug.Log("got the goods!");
        }
        if (babyanimationcontrol.GetInteger("StepTrigger") == 4) //BackingUp
        {
            StartCoroutine("ButtToChair");
            Debug.Log("back to base!");
        }
        if (babyanimationcontrol.GetInteger("StepTrigger") == 5) //EasyDoesIt
        {
            StartCoroutine("DroppingIt");
            Debug.Log("mission success!");
        }
        if (babyanimationcontrol.GetInteger("StepTrigger") == 6) //Reset Routine
        {
            //camera returns to normal
            //(note, when it returns to normal, it will snap back to the place that the
            //player was looking before they started the pickup animation)
            mouselookscript.XSensitivity = 2f;
            mouselookscript.YSensitivity = 2f;
            StartCoroutine("WaitToStartAgain");

            Dialogue2.Advances = 5;
        }

    }
    
    IEnumerator MoveOverThere()
    {         
        if (mobility != "precrawler")
        {
            while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("SittingPosition") == false)
            {
                yield return null;
            }
        }
        while (theParent.transform.position != parentPOV.transform.position)
        {
            theParent.transform.position = Vector3.MoveTowards(
            theParent.transform.position, parentPOV.transform.position, Time.deltaTime * 4);
            playerhands.transform.position = Vector3.MoveTowards(
            playerhands.transform.position, parentPOVArms.transform.position, Time.deltaTime * 4);
            yield return null;
        }
        babyanimationcontrol.SetInteger("StepTrigger", 3);
        LoadTheAnimation();
    }
    IEnumerator GetInMyHands()
    {
        if (mobility != "precrawler")
        {
            while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("Armature|LiftingAnimation") == false)
            {
                yield return null;
            }
        }
        if (mobility == "precrawler")
        {
            while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("PreCrawlerToLiftOff") == false)
            {
                yield return null;
            }
        }
        theBaby.GetComponent<NavMeshAgent>().enabled = false;
        while (BabyNonNavMesh.transform.position != playerhands.transform.position)
        {
            BabyNonNavMesh.transform.position = Vector3.MoveTowards(
            BabyNonNavMesh.transform.position, playerhands.transform.position, Time.deltaTime * 3);
            yield return null;
        }
        
        babyanimationcontrol.SetInteger("StepTrigger", 4);
        LoadTheAnimation();
        
    }

    IEnumerator ButtToChair()
    {


        while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("LiftedPosition") == false)
        {
            yield return null;
        }
        while (theParent.transform.position != parentReturnPoint.transform.position)
        {
            theBaby.transform.position = playerhands.transform.position; //not quite sure why, but this can't be the nonnavmesh object
            theParent.transform.position = Vector3.MoveTowards(
            theParent.transform.position, parentReturnPoint.transform.position, Time.deltaTime * 3);
            playerhands.transform.position = Vector3.MoveTowards(
            playerhands.transform.position, parentHandReturnPoint.transform.position, Time.deltaTime * 3);
            yield return null;
        }
        
        babyanimationcontrol.SetInteger("StepTrigger", 5);
        LoadTheAnimation();
    }

    IEnumerator DroppingIt()
    {

        while (theBaby.transform.position != ReturnPoint.transform.position)
        {
            theBaby.transform.position = Vector3.MoveTowards(
            theBaby.transform.position, ReturnPoint.transform.position, Time.deltaTime * 1);
            yield return null;
        }

        babyanimationcontrol.SetInteger("StepTrigger", 6);
        LoadTheAnimation();
    }


    IEnumerator WaitToStartAgain()
    {
        if (mobility != "precrawler")
        {
            if (mobility == "crawler")
            {
                while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("ReturnToCrawlAnimation") == false)
                {
                    yield return null;
                }
            }
            if (mobility == "walker")
            {
                while (babyanimationcontrol.GetCurrentAnimatorStateInfo(0).IsName("ReturnToWalking") == false)
                {
                    yield return null;
                }
            }
            int xPos = Random.Range(249, 262);
            int zPos = Random.Range(178, 195);
            theDestination.transform.position = new Vector3(xPos, 1, zPos);
        }
        theBaby.GetComponent<NavMeshAgent>().enabled = true;
        theBaby.GetComponent<NavMeshAgent>().isStopped = false;
        babyanimationcontrol.SetInteger("StepTrigger", 0);
        Debug.Log("huzzuh!");
    }

}
