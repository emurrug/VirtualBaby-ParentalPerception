using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoggingTutorial : MonoBehaviour
{
    //the redundancies in this script are phenomenal and painful :')
    public string EVENTNAME;
    public string RESPONSENAME;
    public string BLOCKNAME;
    public string CARSPEED;
    public string TRIAL;

    public ExcelConnect excelconnect;
    public EmptyWorldTutorial emptyworldtutorial;
    public AdvanceScenes advancescenes;
    public RaiseHand raisehand;
    public TwoHandGrabInteractable twohandgrabinteractable;
    public HardwareTracking harwaretracking;

    public bool boolchecker; //checks to see if bools are being activated (set to true if it is actively listening)

    void Start()
    {
        //registers current scene for data logging
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        excelconnect.Save();


        //in an older version of the script, i used to have the "AddListener()" function for events
        //evidently that does not work on bools, so I adopted a clunkier method to use another bool 
        //to turn on and off when the Update function is "listening" for game events
        //the problem is that it turns back on within 1 second of a completed action, so it cannot track multiple
        //things happening within the same 1s timeframe
        boolchecker = true;
    }

    void Update()
    {
        //updates the static variables in CombinedData script
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.CARSPEED = CARSPEED;
        CombinedData.EVENTNAME = EVENTNAME;
        CombinedData.RESPONSENAME = RESPONSENAME;

        //always keeps carspeed up to date
        CARSPEED = emptyworldtutorial.carspeed.ToString();

        if (boolchecker == true)
        {
            //updates data if raised hand
            if (raisehand.Rhigher == true)
            {
                boolchecker = false;
                StartCoroutine("RaiseRHand");
            }
            if (raisehand.Lhigher == true)
            {
                boolchecker = false;
                StartCoroutine("RaiseLHand");
            }

            //updates if L hand touches ball (but not right)
            if (twohandgrabinteractable.RightHandTouch == false && twohandgrabinteractable.LeftHandTouch == true)
            {
                boolchecker = false;
                StartCoroutine("LHandTouch");
            }

            //updates if R hand touches ball (but not left)
            if (twohandgrabinteractable.RightHandTouch == true && twohandgrabinteractable.LeftHandTouch == false)
            {
                boolchecker = false;
                StartCoroutine("RHandTouch");
            }

            //updates if both hands touch (but not grab)
            if (twohandgrabinteractable.TwoHandGrab == false && twohandgrabinteractable.TwoHandTouch == true)
            {
                boolchecker = false;
                StartCoroutine("TwoHandTouch");
            }

            //updates if both hands grab (decided to add the specificity of which ball 
            if (twohandgrabinteractable.TwoHandGrab == true)
            {
                boolchecker = false;
                StartCoroutine("TwoHandGrab");
            }
            //updates when car starts/stops
            if (emptyworldtutorial.car.transform.position == emptyworldtutorial.carEnd.transform.position)
            {
                boolchecker = false;
                StartCoroutine("CarAtEnd");
            }
            if (emptyworldtutorial.car.transform.position == emptyworldtutorial.carStart.transform.position)
            {
                boolchecker = false;
                StartCoroutine("RaiseLHand");
            }
        }
    }

    IEnumerator RaiseRHand()
    {
        EVENTNAME = "Hand Raised";
        RESPONSENAME = "Right";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
       // yield return null;
    }
    IEnumerator RaiseLHand()
    {
        EVENTNAME = "Hand Raised";
        RESPONSENAME = "Left";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator LHandTouch()
    {
        EVENTNAME = "Hand Touching Object";
        RESPONSENAME = "Left Hand Only";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator RHandTouch()
    {
        EVENTNAME = "Hand Touching Object";
        RESPONSENAME = "Right Hand Only";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator TwoHandTouch()
    {
        EVENTNAME = "Hand Touching Object";
        RESPONSENAME = "Two Handed Touch Only";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator TwoHandGrab()
    {
        EVENTNAME = "Hand Touching Object";
        RESPONSENAME = "Two Handed Grab";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator CarAtStart()
    {
        EVENTNAME = "Car Starting";
        RESPONSENAME = "NA";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
    IEnumerator CarAtEnd()
    {
        EVENTNAME = "Car Reached End of Lane";
        RESPONSENAME = "NA";
        excelconnect.Save();
        yield return new WaitForSeconds(1);
        boolchecker = true;
    }
}


