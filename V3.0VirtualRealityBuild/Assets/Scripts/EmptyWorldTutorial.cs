using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EmptyWorldTutorial : MonoBehaviour
{

    //the goal of this script is to define the behavior of 4 different objects:
    //1. The "car" - this ball moves at 3 predetermined speeds
    //      so that subs have speed references
    //2. Still ball - this ball is small and doesn't move;
    //      it gets the sub familiar with playing with objects
    //      04.07.23 RIP still ball (mechanics of switching from one to two hands is too difficult)
    //3. Moving ball - this ball rolls around using the same wander script as the baby;
    //      it gets the sub familiar with picking up moving objects
    //4. Big ball - this ball is twice as large as the others and needs 2 hand pickup;
    //      it gets the sub familiar with the 2 hand grab dynamic

    /* 11.16.2023 Update on tutorial instructions:
     * The goal is to get the sub familiar with the basic gestures/operations of the VR set.
     * 
     * There are 2 main actions the sub need to practice to trigger the events:
     * 1. "Wave" - for the sake of simplicity, anytime when either one of the Controllers is above HMD, triggers the detection of waving action.
     * 2. "Grab" - pulling the back trigger on both Controllers will trigger the handgrabbing animation, only if both hands collide with the object
     *      and are in the handgrabbing animation, the sub can pick up and hold the object. Once the trigger is released, drop the object.
     * 
     * There are 4 main instruction sets (refer to the specific function below for more detail):
     * 1. *set 0 - start: messages occur at the very beginning + first trial of wave (implement a neutral sound as detection feedback?)
     * 2. set 1 - grabbing mechanism tryout
     * 3. set 2 - picking up while rolling tryout
     * 4. set 3 - speed perception and waving mechanism tryout
     */

    public int xPos;
    public int zPos;

    public GameObject theDestination;
    public GameObject roller;
    public GameObject big;
    public GameObject car;
    public GameObject carEnd;
    public GameObject carStart;
    public float rollerspeed;
    public int carspeed;

    public GameObject instructions0; 
    //list of audio files for game instructions:
    public GameObject ping;
    public GameObject welcome;
    public GameObject dropball;
    public GameObject pickuproller;
    public GameObject rollingballspeed;
    public GameObject rollingballRT;
    public GameObject continueon;

    public bool firstwave;
    public bool BallListener;
    public bool ballisrolling;

    //using similar logic to the CarPath Script, the ball start and stop points are ~490ft away from each other
    //the 50 unit travels the distance in about 5s (which is close enough to 50 mph that I am not going back and changing it)
    public List<int> speedslist = new List<int>() { 30, 50, 70 };
    public TMP_Text speedtext;

    public RaiseHand raisehand;

    public int carnumber = 0;

    private bool rollerinboth;
    private bool biginboth;
    private bool dropthefirstball = false;


    //eventually, the scene will start with none of the objects active,
    //and then gradually introduce them as subs complete the goals. 
    //for now, lets start by making them all active, then going back to add activate triggers
    void Start()
    {
        rollerspeed = 2f;
        carspeed = 50;

        //turning off balls
        roller.active = false;
        big.active = false;
        car.active = false;

        //turning on instruction text on canvas
        //01.06.24 update: all instructions are via audio now
        instructions0.active = true;
        ping.active = false;
        welcome.active = false;
        ping.active = false;
        welcome.active = false;
        dropball.active = false;
        pickuproller.active = false;
        rollingballspeed.active = false;
        rollingballRT.active = false;
        continueon.active = false;

        //setting up bools
        firstwave = true;
        BallListener = true;
        ballisrolling = true;
    }

    void Update()
    {
        //get XR interactor components
        bool rollerinboth = roller.GetComponent<TwoHandGrabInteractable>().TwoHandGrab;
        bool biginboth = big.GetComponent<TwoHandGrabInteractable>().TwoHandGrab;

        /* waits for a wave to trigger the game
         * eventually this should be some sort of motion or other natural cue
         * 11.16.2023 Update - wave now should be triggered by raising one hand above the head
         * very much like a "taxi wave" as Emma would say :D
        */

        if (raisehand.Rhigher || raisehand.Lhigher)
        {
            if (firstwave == true)
            {
                StartFirstInstructionSet();
                ping.active = true;
            }
            else
            {
                StartCoroutine("Ping"); 
            }
            //if they raise their hand to move on to the next phase: 
            if (continueon.active == true)
            {
                if (raisehand.Rhigher)
                {
                    //advance scenes to baby scene
                    SceneManager.LoadScene(1);
                }
                if (raisehand.Lhigher)
                {
                    //repeat the tutorial
                    SceneManager.LoadScene(0);
                }
            }
        }


        //waits for specific interactions to trigger object introductions
        if (biginboth == true)
        {
            //need to come back and add a condition so it doesnt keep playing over and over.
            StartSecondInstructionSet();
        }
        if(biginboth ==false && dropthefirstball ==true)
        {
            WaitForBallToDrop();
        }
        if (rollerinboth == true)
        {
            ballisrolling = false; //this is to turn off the rolling sequence so that it doesn't bug out between 2 transforms
            StartCoroutine("StartThirdInstructionSet");
        }
        if (rollerinboth == false)
        {
            ballisrolling = true;
        }



        //the car
        if (car.transform.position != carEnd.transform.position && car.active == true)
        {
            MoveTowards();
        }
        else if (car.transform.position == carEnd.transform.position)
        {
            car.transform.position = carStart.transform.position;
            speedslist.Remove(carspeed);
            carnumber++;
            StartCoroutine("RestartCar");
            //resets the speedlist after the initial detecting speed phase
        }
        speedtext.text = carspeed + "mph";



        //rolling ball
        if (ballisrolling = true)
        {
            roller.transform.position = Vector3.MoveTowards(roller.transform.position, theDestination.transform.position, rollerspeed * Time.deltaTime);
            if (roller.transform.position == theDestination.transform.position)
            {
                StartCoroutine("SetRandomLocation");
            }
        }
    }


    //First phase of instruction begins after firstwave detected
    void StartFirstInstructionSet()
    {
        instructions0.active = false;
        ping.active = false;
        big.active = true;
        //eventually should put instructions here with hand animations for the buttons
        welcome.active = true;
        firstwave = false;
    }

    void StartSecondInstructionSet()
    {
        //eventually should put instructions here with hand animations for the buttons
        dropball.active = true;
        dropthefirstball = true;
    }
    void WaitForBallToDrop()
    {
        roller.active = true;
        pickuproller.active = true;
        dropthefirstball = false;
    }
    IEnumerator StartThirdInstructionSet()
    {
    rollingballspeed.active = true;
    yield return new WaitForSeconds(15);
    car.active = true;
    BallListener = false;

    }
    IEnumerator StartFourthInstructionSet()
    {
        //reset the speed list
        speedslist.Add(30);
        speedslist.Add(50);
        speedslist.Add(70);
        rollingballRT.active = true;
        yield return new WaitForSeconds(26);
        car.active = true;
    }
    void StartFinalInstructionsSet()
    {
        continueon.active = true;
        car.active = false;
    }
    //car movers and roller destinations
    void MoveTowards()
    {
        car.transform.position = Vector3.MoveTowards(car.transform.position, carEnd.transform.position, carspeed * Time.deltaTime);
        //for the car rolling down the lane, i should make a little speed for the UI in the corner to show how fast it is
    }
    IEnumerator RestartCar()
    {
        //lets make this a count instead, everytime it restarts, i add the count
        if (carnumber != 3 && carnumber != 6)
        {
            carspeed = speedslist[Random.Range(0, (speedslist.Count))];
        }      
        if (carnumber == 3)
        {
            car.active = false;
            StartCoroutine("StartFourthInstructionSet");
        }
        if (carnumber == 6)
        {
            StartFinalInstructionsSet();
        }
        yield return null;
    }
    IEnumerator SetRandomLocation()
    {
        xPos = Random.Range(249, 261);
        zPos = Random.Range(175, 185);
        theDestination.transform.position = new Vector3(xPos, 1f, zPos);
        yield return null;
    }
    IEnumerator Ping()
    {
        ping.active = true;
        yield return new WaitForSeconds(1);
        ping.active = false;
    }

}