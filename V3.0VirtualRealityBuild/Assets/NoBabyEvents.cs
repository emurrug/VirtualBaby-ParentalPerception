using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NoBabyEvents : MonoBehaviour
{
    //when carpath.scorevalue = 5; advance to baby scene instructions
    public CarPath carpath;
    public int scoreValue;

    // Update is called once per frame
    void Update()
    {
        scoreValue = CarPath.scoreValue;

        if (scoreValue == 5)
        {
            SceneManager.LoadScene(3);
        }

    }
}
