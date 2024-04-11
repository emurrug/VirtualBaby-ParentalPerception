//this script is to log data in the scene called "Narrative2" (i.e., tutorial for picking up baby)

using System.Collections;
using UnityEngine;

public class Narrative2 : MonoBehaviour
{
    public string EVENTNAME;
    public string RESPONSENAME;
    public string BLOCKNAME;
    public string CARSPEED;
    public string BABYLOCATION;
    public string TRIAL;
    public string CLOTHES;
    public string MOBILITY;

    public ExcelConnect excelconnect;
    public TutorialCarCycle tutorialcarcycle;
    public AdvanceScenes advancescenes;
    public Dialogue2 dialogue2;
    public CopyPickUpBaby pickupbaby;
    public CrossingThreshold crossingthreshold;
    public babymobility babymobility;
    public ScoreKeeper scorekeeper;


    void Start()
    {
        //registers current scene for data logging
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        EVENTNAME = "Start New Block";
        StartCoroutine("Insert");


        //uses button as tricgger to add see if subs need to repeat instructions
        dialogue2.repeatInstructions.onClick.AddListener(RepeatInstructions);
        //uses button as tricgger to add see if subs need to repeat instructions
        dialogue2.imReady.onClick.AddListener(DoNotRepeatInstructions);

        //baby randomizer results
        MOBILITY = StaticMobility.mobility.ToString();
        CLOTHES = StaticMobility.clothes.ToString();

        if (scorekeeper.scoreValue == 5)
        {
            //randomizes baby choice
            babymobility.GiveMeANewBaby();
        }
        if (scorekeeper.scoreValue == 15)
        {
            babymobility.GiveMeANewBaby();
        }
        if (scorekeeper.scoreValue == 25)
        {
            babymobility.GiveMeANewBaby();
        }

    }

    private void Update()
    {   //updates the static variables in CombinedData script
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.CARSPEED = CARSPEED;
        CombinedData.TRIAL = "Tutorial Instructions 2";
        CombinedData.BABYLOCATION = BABYLOCATION;
        CombinedData.EVENTNAME = EVENTNAME;
        CombinedData.RESPONSENAME = RESPONSENAME;
        CombinedData.CLOTHES = CLOTHES;
        CombinedData.MOBILITY = MOBILITY ;


        //updates data if subjects press B to pick up baby
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            CARSPEED = tutorialcarcycle.speed.ToString();
            EVENTNAME = "B Pressed";
            RESPONSENAME = ("Baby Location: " + pickupbaby.BabyNonNavMesh.transform.position);
            StartCoroutine("Insert");
        }

        if (crossingthreshold.BlanketCross == true)
        {
            crossingthreshold.BlanketCross = false;
            CARSPEED = tutorialcarcycle.speed.ToString();
            EVENTNAME = "Baby Crossed Blanket";
            RESPONSENAME = "NA";
            StartCoroutine("Insert");
        }
        if (crossingthreshold.RoadCross == true)
        {
            crossingthreshold.RoadCross = false;
            CARSPEED = tutorialcarcycle.speed.ToString();
            EVENTNAME = "Baby Crossed Road";
            RESPONSENAME = "NA";
            StartCoroutine("Insert");
        }

        if (crossingthreshold.theBlanket == true)
        {
            BABYLOCATION = "Blanket";
        }
        if (crossingthreshold.theGrass == true)
        {
            BABYLOCATION = "Grass";
        }
        if (crossingthreshold.theRoad == true)
        {
            BABYLOCATION = "Road";
        }
    }

    void RepeatInstructions()
    {   //logs if player needed to repeat the instructions
        EVENTNAME = "Repeat Instructions";
        RESPONSENAME = "Yes";
        StartCoroutine("Insert");
    }
    void DoNotRepeatInstructions()
    {   //logs if player is ready to continue
        EVENTNAME = "Repeat Instructions";
        RESPONSENAME = "No";
        StartCoroutine("Insert");
    }

    IEnumerator Insert()
    {
        yield return new WaitForFixedUpdate(); //added redunandance for peace of mind (makes it wait 1 frame)
        excelconnect.Save();
    }

}