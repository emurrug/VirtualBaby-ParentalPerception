//This script programs how the car move down the path during the NeutralScene and the BabyScene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class CarCycle : MonoBehaviour
{
    //beginning and end of car path
    //there are collider objects at these points that the car moves towards/returns to
    public Transform theOrigin;
    public Transform theDestination;
    public bool ReachedDestination = false;
    public bool CarAtOrigin = true;

    public int speed; //how fast the car is moving
    // A note about car speeds: Each unit (the default unity grid) corresponds approximately to 1 ft. The distance
    //between origin and destination is 258.81 ft. This means when "speed = 258", this means the car reaches its destination in ~1sec.
    //The speeds below correspond to ft/s conversions (rounded) of 30, 40, 50, 60, and 70 mph.
    public List<int> speedslist1 = new List<int>() { 44, 59, 73, 88, 103, 44, 59, 73, 88, 103 };
    public List<int> speedslist2 = new List<int>() { }; //empty list to move all of speedslist1 into so speeds aren't repeated (i.e., randomization without replacement)

    public static int scoreValue = 0; //how many times the player correctly hit SPACE to wave down cars
    public bool AlreadyScored = false; //prevents subs from spamming SPACE more than once per trial
    public int trial = 1; //increases each time the car loops back over (includes both "misses" and "hits")

    public int randomdelay; //to make car onset unpredictable to viewers
    public string carlocation; //logs beginning and end of car transit to track timing

    //these elements are also in the "Dialogue" script, and control the speed entry box
    public GameObject SpeedBox; //note 01.25: this object changed from an input field to a slider
    public Button enterMPH;
    //public InputField mphInputField;
    public Slider LikertSlider;

    //outside scripts that get referenced
    public AdvanceScenes advancescenes;
    public NeutralScene neutralscene;
    public BabyScene babyscene;
    public babymobility babymobility;
    public ScoreKeeper scorekeeper;
    //public Dialogue2 dialogue2;

    //the hand visual when the car is flagged down
    public GameObject theHand;

    //this standard asset script is referenced to force camera positioning during mph answering
    public MouseLook mouselookscript;
    public RigidbodyFirstPersonController theParent; //the cylindrical game object with the camera attached

    void Awake()
    {
        //chooses a random wait time for the first trial
        StartCoroutine("RandomWaitDelay");
    }

    void Start()
    {
        //turns off speed entry box
        SpeedBox.gameObject.SetActive(false);

        //turns on trigger when sub enters their speed estimation
        Button submitmph = enterMPH.GetComponent<Button>();
        submitmph.onClick.AddListener(StartLoopOver);

        //makes sure the hand object is turned off until needed
        theHand.gameObject.SetActive(false);

        //this standard asset script is referenced to prevent camera movement when entering speed
        mouselookscript = theParent.mouseLook;

        //game test
        //Debug.Log("Index1:" + speedslist1.IndexOf(speed));
        //Debug.Log("Speed:" + speed);

	AlreadyScored = false;
    }

    void Update()
    {
        //moves car towards destination
        if (ReachedDestination == false && CarAtOrigin == false)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                theDestination.position, Time.deltaTime * speed);

        }

        //adds a point to score if they press SPACE while the car is moving
        if (Input.GetKeyDown(KeyCode.Mouse2) && ReachedDestination == false &&
            CarAtOrigin == false && AlreadyScored == false)
        {
            scoreValue += 1;
            AlreadyScored = true; //prevents re-scoring

            //turns on the waving hand object
            StartCoroutine("WaveTheHand");

        }

        //when the car reaches destination, stop and answer mph question
        if (transform.position == theDestination.position)
        {
            ReachedDestination = true;
            SpeedBox.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            mouselookscript.XSensitivity = 0f;
            mouselookscript.YSensitivity = 0f;
        }
    }

    //if the car reaches the end of the track, it gets updated and logged
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CarDestination")
        {
            carlocation = "Car Ends";
            if (advancescenes.block == "4.NeutralScene")
            {
                neutralscene.GetCarLocation();
            }
            else if (advancescenes.block == "6.BabyScene")
            {
                babyscene.GetCarLocation();
            }

        }
    }

    //When the car resets, it has to wait for a random amount of time (currently between 5-10 seconds)
    IEnumerator RandomWaitDelay()
    {
        CarAtOrigin = true;
        randomdelay = Random.Range(5, 15);
        yield return new WaitForSeconds(randomdelay);
        speed = speedslist1[Random.Range(0, (speedslist1.Count))]; //adjusted this 02/17/22 to reflect the entire range of the list

        CarAtOrigin = false;
        //mphInputField.text = "0";
        LikertSlider.value = 4;
        carlocation = "Car Starts";
        if (advancescenes.block == "4.NeutralScene")
        {
            neutralscene.GetCarLocation();
        }
        else if (advancescenes.block == "6.BabyScene")
        {
            babyscene.GetCarLocation();
        }
    }


    //the loop starts over after each trial
    public void StartLoopOver()
    {
        //this stuff happens every time...
        trial += 1;
        mouselookscript.XSensitivity = 2f;
        mouselookscript.YSensitivity = 2f;
        StartCoroutine("RandomWaitDelay");
        SpeedBox.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ReachedDestination = false;
        transform.position = theOrigin.transform.position;

        //after the tutorial scene, they move onto the new baby scenes
        //there should be a baby selection randomizer at in the coroutine "GoToBabyScene"
        if (AlreadyScored == true) //has to be here to prevent rescoring if they miss a car on 5, 15, or 25
	{
	if (scoreValue == 5 | scoreValue == 15 | scoreValue == 25)
        {
        	
		if (scoreValue == 5)
	    	{
		advancescenes.GoToBabyInstructions();
		}
		if (scoreValue == 15)
	    	{
		Dialogue2.Advances = 6;
		advancescenes.GoToBabyInstructions();
		}
        	if (scoreValue == 25)
	    	{
		Dialogue2.Advances = 10;
		advancescenes.GoToBabyInstructions();
		}
		//need to set up a coroutine so that there is a time gap between getMPH command and GoToBabyinstructions
        }
        //advances to end of game instructions
        else if (scoreValue == 35)
        {
            advancescenes.GoToEndGame();
        }
        //otherwise here is how the speeds are randomly selected without replacement
        //if they do not score, that speed remains on the list to ensure that they see it
        else
        {
            //identifies the current speed and removes it from the current speedslist
            int speedslist1index = speedslist1.IndexOf(speed);
            int temp1 = speedslist1[speedslist1index];
            //and moves it to the second list only if they waved the car down

           	speedslist2.Add(temp1);
                speedslist1.RemoveAt(speedslist1index);
		AlreadyScored = false;
	}
        }

        //game test
        //Debug.Log("Index1:" + speedslist1.IndexOf(speed));
        //Debug.Log("Speed:" + speed);

    }

    IEnumerator WaveTheHand()
    {
        theHand.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        theHand.gameObject.SetActive(false);
    }


}

