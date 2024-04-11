using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RobotInstructionsEvent : MonoBehaviour
{
    public RaiseHand raisehand;
    public bool waitingasec;
    void Start()
    {
        waitingasec = true;
        StartCoroutine("WaitASec");
    }
    
    void Update()
    {
        if (raisehand.Rhigher == true && waitingasec == false)
        {
            SceneManager.LoadScene(6);
        }
    }
    IEnumerator WaitASec()
    {
        yield return new WaitForSeconds(10);
        waitingasec = false;
    }

}
