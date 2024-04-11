using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;

public class RaiseHand : MonoBehaviour
{
    //purpose of this script is to look at the HardwareTracking script and determine if someone is
    //raising their hand over their head

    public HardwareTracking hardwaretracking;

    //our main axis is going to be the y-axis here
    //so let's just start by getting the Y pos for each of the hand + head vectors
    //then running an update to always check if either hand is higher than head
    //can set up 4 bools to show what is going on

    public bool Rhigher;
    public bool Lhigher;
    public bool BothHigher;
    public bool BothDown; //(i.e., head is highest)

    public float leftHeight = 0f;
    public float rightHeight = 0f;
    public float headHeight = 0f;


    void Update()
    {
        //lazy :(
        //obtain the y-axis value for leftController, rightController, and HMD
        //note: controllers can start to be tracking once putting on the HMD
        leftHeight = hardwaretracking.LHand.y;
        rightHeight = hardwaretracking.RHand.y;       
        headHeight = hardwaretracking.Head.y;

        //Conditions check:
            if (leftHeight > headHeight)
            {
                Lhigher = true;
            } else
            {
                Lhigher = false;
            }
            if (rightHeight > headHeight)
            {
                Rhigher = true;
            } else
            {
                Rhigher = false;
            }
            if (Rhigher == true && Lhigher == true)
            {
                BothHigher = true;
            } else
            {
                BothHigher = false;
            }
            if (Rhigher == false && Lhigher == false)
            {
                BothDown = true;
            } else
            {
                BothDown = false;
            }
     
    }
}
