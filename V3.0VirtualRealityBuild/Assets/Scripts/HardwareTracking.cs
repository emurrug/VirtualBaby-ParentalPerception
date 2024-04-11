
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.XR;
using System.Collections.Generic;

public class HardwareTracking : MonoBehaviour
{
    public string BLOCKNAME;
    public string EVENTNAME;
    public string GAZE;
    public string GAZECATEGORY;
    public string RHANDROT;
    public string LHANDROT;
    public string RHANDLOC;
    public string LHANDLOC;
    public string HEADROT;
    public string HEADLOC;

    public ExcelConnect excelconnect;
    public AdvanceScenes advancescenes;
    public ViveSR.anipal.Eye.SRanipal_GazeRaySample gazeraysample;

    public InputDevice _rightController;
    public InputDevice _leftController;
    public InputDevice _HMD;

    //these are vectors that are being made open to other scripts to view
    //(e.g., to determine if motion actions are happening, like waving)
    public Vector3 RHand;
    public Vector3 LHand;
    public Vector3 Head;

    void Start()
    {
        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        EVENTNAME = "Start New Block";
        StartCoroutine("InsertTrigger");
    }

    void Update()
    {
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.EVENTNAME = EVENTNAME;
        //need to go back and tidy this up in it's own file (not here)
        //note: i can go back and add acceleration/velocity if these tools are valuable for measuring behavior
        if (!_rightController.isValid || !_rightController.isValid || !_HMD.isValid)
        {
            InitializeInputDevices();
        }

        if (_HMD.TryGetFeatureValue(CommonUsages.centerEyePosition, out Vector3 centerEyePositon))
        {
            HEADLOC = (centerEyePositon.ToString());
            Head = centerEyePositon;
        }
        if (_HMD.TryGetFeatureValue(CommonUsages.centerEyeRotation, out Quaternion centerEyeRotation))
        {
            HEADROT = (centerEyeRotation.ToString());
        }
        if (_rightController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion RdeviceRotation))
        {
            RHANDROT = (RdeviceRotation.ToString());
        }
        if (_leftController.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion LdeviceRotation))
        {
            LHANDROT = (LdeviceRotation.ToString());
        }
        if (_rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 RdevicePosition))
        {
            RHANDLOC = (RdevicePosition.ToString());
            RHand = RdevicePosition;
        }
        if (_leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 LdevicePosition))
        {
            LHANDLOC = (LdevicePosition.ToString());
            LHand = LdevicePosition;
        }

        GAZE = gazeraysample.GAZE;
        GAZECATEGORY = gazeraysample.GAZECATEGORY;

        CombinedData.GAZE = GAZE;
        CombinedData.GAZECATEGORY = GAZECATEGORY;
        CombinedData.RHANDROT = RHANDROT;
        CombinedData.LHANDROT = LHANDROT;
        CombinedData.RHANDLOC = RHANDLOC;
        CombinedData.LHANDLOC = LHANDLOC;
        CombinedData.HEADROT = HEADROT;
        CombinedData.HEADLOC = HEADLOC;
    }

    private void InitializeInputDevices()
    {
        if(!_rightController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref _rightController);
        }
        if (!_leftController.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref _leftController);
        }
        if (!_HMD.isValid)
        {
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref _HMD);
        }
    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);
        if(devices.Count >0)
        {
            inputDevice = devices[0];
        }
    }

    public void InsertRow()
    {
        StartCoroutine("InsertTrigger");
    }
    IEnumerator InsertTrigger()
    {
        yield return new WaitForFixedUpdate(); //added redunandance for peace of mind
        excelconnect.Save();
    }

}
