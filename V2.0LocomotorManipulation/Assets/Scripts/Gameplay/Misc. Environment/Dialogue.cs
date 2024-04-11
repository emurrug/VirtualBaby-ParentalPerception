//This script is for the scene called "Narrative1"
//It delivers instructions to the subjects about how to wave down the cars
//updated 02.10.22 to add reflect new dialogue with 3 babies

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class Dialogue : MonoBehaviour
{
    public static int Advances = 0;     //how many times the subject has clicked (to read instructions)

    public GameObject DialogueBox;      //the canvas gameobject that has all the instructions elements
    public GameObject SpeedBox;         //the canvas gameobject that has all the elements to input the est. car speed
    public GameObject CarScoreBox;      //the canvas gameobject that presents the current score to subjects

    public GameObject DialogueText;     //the written instructions
    public GameObject ClicktoContinue;  //small text item that lets subjects know how to advance

    public Button enterMPH;             //the button to "submit" their estimated speed in mph
    //public InputField mphInputField;    //where subjects write out their estimated mph
    public Slider LikertSlider;

    public Button repeatInstructions;   //button to repeat instructions over
    public Button imReady;              //button to advance to next scene after instructions

    //this standard asset script is referenced to force camera positioning during mph answering
    public MouseLook mouselookscript;
    public RigidbodyFirstPersonController theParent; //the cylindrical game object with the camera attached

    void Start()
    {
        //for when subjects need to repeat instructions or move on after the instructions
        Button repeat = repeatInstructions.GetComponent<Button>();
        repeat.onClick.AddListener(startover);

        Button moveon = imReady.GetComponent<Button>();
        moveon.onClick.AddListener(GoToTestScene);

        //when they enter their estimated mph, it resolves the practice trial and moves on
        Button submitmph = enterMPH.GetComponent<Button>();
        submitmph.onClick.AddListener(EndPracticeTrial);

        //make sure only the correct items are showing at start
        SpeedBox.gameObject.SetActive(false);
        CarScoreBox.gameObject.SetActive(false);
        repeatInstructions.gameObject.SetActive(false);
        imReady.gameObject.SetActive(false);
        ClicktoContinue.gameObject.SetActive(true);

        //hides the mouse and the camera is locked to mouse tracking
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //this standard asset script is referenced to prevent camera movement when entering speed
        mouselookscript = theParent.mouseLook;
    }


    void Update()
    {   //when subs click, they advance the instructions dialogue
        if (Input.GetKeyDown(KeyCode.Mouse0) && Advances != 11 && Advances != 13)
        {
            Advances += 1;
        }

        //note: the dialogue can be written as a "list" method to make it easier on the eyes, but...I'm just lazy
        if (Advances == 0)
        {
            DialogueText.GetComponent<Text>().text = "You were on your way back home from picking your infant up from the nursery.";
        }
        else if (Advances == 1)
        {
            DialogueText.GetComponent<Text>().text = "A couple of your friends, who also have infants, are sick. They asked you to pick up their children from the nursery as well.";
        }
        else if (Advances == 2)
        {
            DialogueText.GetComponent<Text>().text = "So now you have 3 babies in your car.";
        }
        else if (Advances == 3)
        {
            DialogueText.GetComponent<Text>().text = "On your way to bring the babies home, you suddenly realized you were completely out of gas.";
        }
        else if (Advances == 4)
        {
            DialogueText.GetComponent<Text>().text = "Your car can go no further. ";
        }
        else if (Advances == 5)
        {
            DialogueText.GetComponent<Text>().text = "You pulled over and tried to call help, but unfortunately there is no cell phone signal out here.";
        }
        else if (Advances == 6)
        {
            DialogueText.GetComponent<Text>().text = "You are going to have to wave down cars and hope someone stops to help. ";
        }
        else if (Advances == 7)
        {
            DialogueText.GetComponent<Text>().text = "Fortunately, you have a blanket and folding chair in the backseat of your car, so you can sit as you wait for cars.";
        }
        else if (Advances == 8)
        {
            DialogueText.GetComponent<Text>().text = "The babies are all sleeping too, so you hope you can wave down the cars before any of them get fussy.";
        }
        else if (Advances == 9)
        {
            DialogueText.GetComponent<Text>().text = "Gameplay Instructions:" + Environment.NewLine + "Use your mouse to look around.";
        }
        else if (Advances == 10)
        {
            DialogueText.GetComponent<Text>().text = "Make sure to wave to the cars as soon as you see them. Go as fast as possible! They will not see you if you wait too long.";
        }
        else if (Advances == 11) //will not advance until they score (see "TutorialCarCycle" script)
        {
            DialogueText.GetComponent<Text>().text = "Press [MIDDLE MOUSE BUTTON] to wave at oncoming cars."
                + Environment.NewLine + "Try it on the next car you see!";
            CarScoreBox.gameObject.SetActive(true);
            ClicktoContinue.gameObject.SetActive(false);
        }

        else if (Advances == 12)
        {
            DialogueText.GetComponent<Text>().text = "Great! Wave down 35 cars to win the game! Go ahead and click now to continue.";
            ClicktoContinue.gameObject.SetActive(true);
        }
        else if (Advances == 13) //will not advance without pressing a button
        {
            //ends isntructions and turns on the buttons
            DialogueText.GetComponent<Text>().text = "Do these instructions make sense?";
            repeatInstructions.gameObject.SetActive(true);
            imReady.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ClicktoContinue.gameObject.SetActive(false);
        }

        //waits until subjects have successfully waved down a car to advance
        if (TutorialCarCycle.scoreValue == 1 && Advances == 11)
        {
            DialogueBox.gameObject.SetActive(false);
            SpeedBox.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouselookscript.XSensitivity = 0f;
            mouselookscript.YSensitivity = 0f;

        }
    }

    // the subject has just submitted their practice mph estimation
    public void EndPracticeTrial()
    {
        Advances += 1;
        DialogueBox.gameObject.SetActive(true);
        SpeedBox.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouselookscript.XSensitivity = 2f;
        mouselookscript.YSensitivity = 2f;
    }
    
    //the subject has opted to repeat all the instructions
    void startover()
    {
        Advances = 0;
        LikertSlider.value = 3;
        //mphInputField.text = "0"; //refreshes the input field
        TutorialCarCycle.scoreValue = 0;

        repeatInstructions.gameObject.SetActive(false);
        imReady.gameObject.SetActive(false);
    }
    
    //the subject has opted to move on to the next game scene
    void GoToTestScene()
    {
        SceneManager.LoadScene(4);
        Advances = 0;
        
    }
}
