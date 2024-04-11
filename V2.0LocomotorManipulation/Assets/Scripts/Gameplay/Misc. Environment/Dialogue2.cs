//This script is for the scene called "Narrative2"
//It delivers instructions to the subjects about how to pick up the baby
//02.14.21 made edits to reflect different dialogues between different mobility types

using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class Dialogue2 : MonoBehaviour
{
    public static int Advances = 0;     //how many times the subject has clicked (to read instructions)

    public GameObject DialogueBox;      //the canvas gameobject that has all the instructions elements

    public GameObject DialogueText;     //the written instructions
    public GameObject ClicktoContinue;  //small text item that lets subjects know how to advance

    public Button repeatInstructions;   //button to repeat instructions over
    public Button imReady;              //button to advance to next scene after instructions
    public GameObject SpeedBox;

    public babymobility babymobility;
    public ScoreKeeper scorekeeper; 


    void Awake() //THIS HAS TO BE AWAKE (it needs it initialize before CopyPickUpBaby.cs for BabyMobility.cs to have an entry for mobility/clothes
    {
        //for when subjects need to repeat instructions or move on after the instructions
        Button repeat = repeatInstructions.GetComponent<Button>();
        repeat.onClick.AddListener(startover);

        Button moveon = imReady.GetComponent<Button>();
        moveon.onClick.AddListener(GoToBabyScene);

        //make sure only the correct items are showing at start
        SpeedBox.gameObject.SetActive(false);
        repeatInstructions.gameObject.SetActive(false);
        imReady.gameObject.SetActive(false);
        ClicktoContinue.gameObject.SetActive(true);


        //hides the mouse and the camera is locked to mouse tracking
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	
	Debug.Log("Advance: " + Advances);
        if (Advances == 0)
        {
            babymobility.AddClothingOptions(); //creates list of clothing textures and mobility options to randomize from
            babymobility.GiveMeANewBaby(); //randomly selects mobility and clothes, then removes it from respective lists

        }
        if (Advances == 6 | Advances == 10)
        {
            babymobility.GiveMeANewBaby(); //selects mobility/clothes whenever new tutorial is called
		Debug.Log("I have selected a new baby!");
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && Dialogue.Advances == 4)
        //when subs pressed B, they advance the instructions dialogue
        {
            Advances = 5;
            Debug.Log("Advance: " + Advances);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && Advances != 4 &&
        Advances != 5 && Advances != 9 && Advances != 13)
        {
            Advances += 1;
        Debug.Log("Advance: " + Advances);
        }
        if (Advances == 0)
        { DialogueText.GetComponent<Text>().text = "You were able to flag down 5 cars before one of the babies started to get fussy"; }
        else if (Advances == 1)
        { DialogueText.GetComponent<Text>().text = "To prevent the baby from waking the other two, you brought them out of the car to be with you as you kept waving down cars."; }

        else if (Advances == 2 & babymobility.mobility != "precrawler")
        { DialogueText.GetComponent<Text>().text = "If your baby gets too close to the road, you can always pick them up and bring them back in."; }
        else if (Advances == 2 & babymobility.mobility == "precrawler")
        { DialogueText.GetComponent<Text>().text = "If you want, you can pick up your baby to check on them and set them back down."; }

        else if (Advances == 3)
        { DialogueText.GetComponent<Text>().text = "However, if you are picking up your baby, you might not see oncoming cars."; }
        else if (Advances == 4) //will not advance until they pick the baby up (see "PickUpBaby" script)
        {
            DialogueText.GetComponent<Text>().text = "Press [RIGHT MOUSE BUTTON] to pick up your baby."
                + Environment.NewLine + "Try it now!";
            ClicktoContinue.gameObject.SetActive(false);
        }
        else if (Advances == 5) //will not advance without pressing a button
        {
            DialogueText.GetComponent<Text>().text = "Are you Ready?";
            //ends isntructions and turns on the buttons
            repeatInstructions.gameObject.SetActive(true);
            imReady.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ClicktoContinue.gameObject.SetActive(false);
        }
        else if (Advances == 6)
        { DialogueText.GetComponent<Text>().text = "The baby with you was getting tired right as another baby in the car started getting fussy"; }
        else if (Advances == 7)
        { DialogueText.GetComponent<Text>().text = "You decided to switch them out so you could keep waving down cars."; }
        else if (Advances == 8)
        { DialogueText.GetComponent<Text>().text = "Remember, you can always pick up the baby if you need to by looking at them and pressing [RIGHT MOUSE BUTTON]"; }
        else if (Advances == 9) //will not advance without pressing a button
        {
            DialogueText.GetComponent<Text>().text = "Are you Ready?";
            //ends isntructions and turns on the buttons
            repeatInstructions.gameObject.SetActive(true);
            imReady.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ClicktoContinue.gameObject.SetActive(false);

        }
        else if (Advances == 10)
        { DialogueText.GetComponent<Text>().text = "The last baby in the car started getting fussy and wanted out."; }
        else if (Advances == 11)
        { DialogueText.GetComponent<Text>().text = "Just to be safe, you put the baby that was with you back in the carseat."; }
        else if (Advances == 12)
        { DialogueText.GetComponent<Text>().text = "That way you only had to watch one baby at a time."; }
        else if (Advances == 13) //will not advance without pressing a button
        {
            DialogueText.GetComponent<Text>().text = "Only 10 more cars to go!" + Environment.NewLine + "Are you Ready?";
            //ends isntructions and turns on the buttons
            repeatInstructions.gameObject.SetActive(true);
            imReady.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            ClicktoContinue.gameObject.SetActive(false);
        }
	else if (Advances >13)
	{
	startover();	
	}

    }

        //the subject has opted to repeat all the instructions
        void startover()
        {
            if (scorekeeper.scoreValue == 5)
            {
                Advances = 0;
            }
            if (scorekeeper.scoreValue == 15)
            {
                Advances = 6;
            }
            if (scorekeeper.scoreValue == 25)
            {
                Advances = 10;
            }

        repeatInstructions.gameObject.SetActive(false);
        imReady.gameObject.SetActive(false);
        ClicktoContinue.gameObject.SetActive(true);
        }

        //the subject has opted to move on to the next game scene
        void GoToBabyScene()
        {
	StartCoroutine("GoToBabyScene2");
        }
    IEnumerator GoToBabyScene2()
    {
        while (Advances == 5)
        {
            Advances = 6;
            yield return null;
            Debug.Log("Advance: " + Advances);
        }
            SceneManager.LoadScene(6);
				
        while (Advances == 9)
        {
            Advances = 10;
            yield return null;
	    Debug.Log("Advance: " + Advances);
        }
        SceneManager.LoadScene(6);
    }
}
