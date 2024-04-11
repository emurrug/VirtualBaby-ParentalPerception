using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotSceneEvent : MonoBehaviour
{

    public BabyWander babywander;
    public RaiseHand raisehand;
    public CarPath carpath;
    public int scoreValue = 0;
    public GameObject GrabbableBabyModel;


    void Update()
    {
        scoreValue = CarPath.scoreValue;

        if (scoreValue >= 25)
        {
            SceneManager.LoadScene(7);
        }

        bool PickedUp = GrabbableBabyModel.GetComponent<TwoHandGrabInteractable>().TwoHandGrab;
        //if the baby is picked up, turn off baby wander script
        if (PickedUp == true)
        {
            babywander.enabled = false;
        }
        if (PickedUp == false)
        {
            babywander.enabled = true;
        }
    }
}
