
using UnityEngine;
using UnityEngine.UI; 


public class TutorialScoreKeeper : MonoBehaviour
{

    public static int scoreValue;
    public GameObject score;


    void Update()
    {
        
        score.GetComponent<Text>().text = "Cars Flagged:" + scoreValue;

        scoreValue = TutorialCarCycle.scoreValue;

    }
}

