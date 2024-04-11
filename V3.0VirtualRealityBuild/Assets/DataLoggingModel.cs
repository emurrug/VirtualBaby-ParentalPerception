using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoggingModel : MonoBehaviour
{
    public string EVENTNAME;
    public string RESPONSENAME;
    public string BLOCKNAME;
    public string CARSPEED;
    public string TRIAL;
    public int trial = 0;

    public ExcelConnect excelconnect;
    public CarPath carpath;
    public AdvanceScenes advancescenes;
    public RaiseHand raisehand;
    public HardwareTracking harwaretracking;
    public TwoHandGrabInteractable twohandgrabinteractable;

    public bool boolchecker; //checks to see if bools are being activated (set to true if it is actively listening)

    void Start()
    {
        //registers current scene for data logging
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        excelconnect.Save();
        boolchecker = true;
    }

    void Update()
    {
        //updates the static variables in CombinedData script
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.CARSPEED = CARSPEED;
        CombinedData.TRIAL = TRIAL;
        CombinedData.EVENTNAME = EVENTNAME;
        CombinedData.RESPONSENAME = RESPONSENAME;

        //always keeps carspeed up to date
        CARSPEED = carpath.speed.ToString();
        TRIAL = trial.ToString();

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
            //not quite the last stop, but the subject can't see it anymore either way
            if (carpath.x == 9)
            {
                boolchecker = false;
                StartCoroutine("CarAtEnd");
            }
            if (carpath.x == 2) //the stopsign is point x = 1, which triggers a 3s wait
            {
                boolchecker = false;
                StartCoroutine("CarAtStart");
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

    IEnumerator CarAtStart()
    {
        EVENTNAME = "Car Starting";
        RESPONSENAME = "NA";
        trial++;
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
}

