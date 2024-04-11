//This script is to record data during the block where only a car is looping (without a baby)
//See "CombinedData" script for more details

using System.Collections;
using UnityEngine;

public class NeutralScene : MonoBehaviour
{
    public string EVENTNAME;
    public string RESPONSENAME;
    public string BLOCKNAME;
    public string CARSPEED;
    public string TRIAL;

    public ExcelConnect excelconnect;
    public ScoreKeeper scorekeeper;
    public CarCycle carcycle;
    public AdvanceScenes advancescenes;


    private void Start()
    {
        //registers current scene for data logging
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        EVENTNAME = "Start New Block";
        //StartCoroutine("Insert"); I decided to remove this to see if it would resolve the conflict
        //with recording the 20th estimated speed

        //uses button as tricgger to add estimated speed to the data file
        carcycle.enterMPH.onClick.AddListener(GetMPH);
        StartCoroutine("Insert");

    }

    void Update()
    {   //updates the static variables in CombinedData script
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.CARSPEED = CARSPEED;
        CombinedData.TRIAL = TRIAL;
        CombinedData.EVENTNAME = EVENTNAME;
        CombinedData.RESPONSENAME = RESPONSENAME;


        //updates data if subjects press SPACE to wave down cars
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            TRIAL = carcycle.trial.ToString();
            CARSPEED = carcycle.speed.ToString();
            EVENTNAME = "Score (Space Pressed)";
            RESPONSENAME = "" + (scorekeeper.scoreValue + 1);
            StartCoroutine("Insert");
            Debug.Log("Space Recorded!");
        }
    }

    public void GetMPH()
    {   //updates data if subjects submit their estimated mph speed
        TRIAL = (carcycle.trial - 1).ToString();
        CARSPEED = carcycle.speed.ToString();
        EVENTNAME = "Estimated Speed";
        RESPONSENAME = carcycle.LikertSlider.value.ToString();
        StartCoroutine("Insert");
        Debug.Log("Speed Recorded = " + carcycle.speed.ToString());

    }

    public void GetCarLocation() //called from "CarCycle" script
    {   //updates data when the car changes location (starts moving and ends moving)
        TRIAL = carcycle.trial.ToString();
        CARSPEED = carcycle.speed.ToString();
        EVENTNAME = carcycle.carlocation;
        RESPONSENAME = "NA";
        StartCoroutine("Insert");
    }
    IEnumerator Insert()
    { 
        yield return new WaitForFixedUpdate(); //added redunandance for peace of mind (makes it wait 1 frame)
        excelconnect.Save();
        Debug.Log("Inserted!");
    }


}



