//This script programs how the car moves down the path during the instructions scenes, Narrative1 and Narrative2

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

//This script is the simplified version of "CarCycle", which plays during normal trials
//The car will loop until the player hits SPACE.
//Only then will it prompt them to enter in the mph speed. 
//Then the script will end.

public class TutorialCarCycle : MonoBehaviour
{
    public Transform theOrigin; //car starting point
    public Transform theDestination; //car ending point
    public bool ReachedDestination = false;


    public int speed; //speed of the car
    public List<int> speedslist = new List<int>() { 44, 59, 73, 88, 103, 44, 59, 73, 88, 103 };
    public int randomdelay = 5; //to make car onset unpredictable to viewers

    //where the subject will enter their estimated speed
    public GameObject SpeedBox;
    public Button enterMPH;
    //public InputField mphInputField;
    public Slider LikertSlider;

    //scoring elements
    public static int scoreValue = 0;
    public bool AlreadyScored = false;

    //the hand visual when the car is flagged down
    public GameObject theHand;



    void Start()
    {
        speed = speedslist[UnityEngine.Random.Range(0, (speedslist.Count - 1))];
        randomdelay = UnityEngine.Random.Range(5, 15);

        //makes sure the hand object is turned off until needed
        theHand.gameObject.SetActive(false);


    }

    void Update()
    {
        //moves car towards destination
        if (ReachedDestination == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, theDestination.position, Time.deltaTime * speed);
        }

        //adds a point if they press space while the car is moving
        if (Input.GetKeyDown(KeyCode.Mouse2) && ReachedDestination == false 
            && AlreadyScored == false && Dialogue.Advances == 11)
        {
            scoreValue += 1;
            AlreadyScored = true;

            //turns on the waving hand object
            StartCoroutine("StartTheHand");
        }

        // starts the loop over when the car reaches the end of the track
        if (transform.position == theDestination.position)
        {
            ReachedDestination = true;
            AlreadyScored = false;
            StartCoroutine("StartLoopOver");
        }

    }

    IEnumerator StartLoopOver()
    {
        //resets the inputfield to 0
        //mphInputField.text = "0";
        LikertSlider.value = 4;

        //put the car back at origin
        transform.position = theOrigin.transform.position;

        //randomly determines speed of the car from "speedslist"
        speed = speedslist[UnityEngine.Random.Range(0, (speedslist.Count - 1))];

        //sets how long of a time there is in between car loops
        randomdelay = UnityEngine.Random.Range(5, 10);
        yield return new WaitForSeconds(randomdelay);

        //returns car to track
        ReachedDestination = false;



    }

    private IEnumerator StartTheHand()
    {
        theHand.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        theHand.gameObject.SetActive(false);
    }
}
