//This script is to record data during the block where the baby is freely crawling
//See "CombinedData" script for more details

using UnityEngine;
using System.Collections;

public class BabyScene : MonoBehaviour
{
    public string EVENTNAME;
    public string RESPONSENAME;
    public string BLOCKNAME;
    public string CARSPEED;
    public string BABYLOCATION;
    public string TRIAL;
    public string MOBILITY;
    public string CLOTHES;

    public ExcelConnect excelconnect;
    public ScoreKeeper scorekeeper;
    public CarCycle carcycle;
    public CopyPickUpBaby pickupbaby;
    public AdvanceScenes advancescenes;
    public CrossingThreshold crossingthreshold;
    public babymobility babymobility;


    private void Start()
    {
        //registers current scene for data logging
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        EVENTNAME = "Start New Block";
        StartCoroutine("Insert");

        //uses button as tricgger to add estimated speed to the data file
        carcycle.enterMPH.onClick.AddListener(GetMPH);
        StartCoroutine("Insert");


        //baby randomizer results
        MOBILITY = StaticMobility.mobility.ToString();
        CLOTHES = StaticMobility.clothes1.ToString();

    }

    void Update()
    {   //updates the static variables in CombinedData script
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.CARSPEED = CARSPEED;
        CombinedData.TRIAL = TRIAL;
        CombinedData.BABYLOCATION = BABYLOCATION;
        CombinedData.EVENTNAME = EVENTNAME;
        CombinedData.RESPONSENAME = RESPONSENAME;
        CombinedData.MOBILITY = MOBILITY;
        CombinedData.CLOTHES = CLOTHES;

        //updates data if subjects press SPACE to wave down cars
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            TRIAL = carcycle.trial.ToString();
            CARSPEED = carcycle.speed.ToString();
            EVENTNAME = "Score (Space Pressed)";
            RESPONSENAME = "" + (scorekeeper.scoreValue);
            StartCoroutine("Insert");
        }
        //updates data if subjects press B to pick up the baby
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            TRIAL = carcycle.trial.ToString();
            CARSPEED = carcycle.speed.ToString();
            EVENTNAME = "B Pressed";
            RESPONSENAME = ("Baby Location: " + pickupbaby.BabyNonNavMesh.transform.position);
            StartCoroutine("Insert");
        }

        if (crossingthreshold.BlanketCross == true)
        {
            crossingthreshold.BlanketCross = false;
            CARSPEED = carcycle.speed.ToString();
            EVENTNAME = "Baby Crossed Blanket";
            RESPONSENAME = "NA";
            StartCoroutine("Insert");
        }

        if (crossingthreshold.RoadCross == true)
        {
            crossingthreshold.RoadCross = false;
            CARSPEED = carcycle.speed.ToString();
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

    public void GetCarLocation()
    {   //updates data when the car changes location (starts moving and ends moving)
        TRIAL = carcycle.trial.ToString();
        CARSPEED = carcycle.speed.ToString();
        EVENTNAME = carcycle.carlocation;
        RESPONSENAME = "NA";
        StartCoroutine("Insert");
    }

    private void GetMPH()
    {   //updates data if subjects submit their estimated mph speed
        TRIAL = (carcycle.trial - 1).ToString();
        CARSPEED = carcycle.speed.ToString();
        EVENTNAME = "Estimated Speed";
        RESPONSENAME = carcycle.LikertSlider.value.ToString();
        StartCoroutine("Insert");
    }

    //needed to include this so the Insert function wasn't running prematurely
    IEnumerator Insert()
    {
        yield return new WaitForFixedUpdate(); //added redunandance for peace of mind (makes it wait one frame)
        excelconnect.Save();
    }
}



