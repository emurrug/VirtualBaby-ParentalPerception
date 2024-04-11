//this script is to help advance scenes
//loadscene ordering can be accessed in "File" >> "Build Settings" within the Unity UI


using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvanceScenes : MonoBehaviour
{
    public string block; //can be referenced to get current block


    void Start()
    {
        CurrentBlock();
    }
    void Update()
    {
        //restricts subs from continuing without the researcher
        //researcher instructs them to hit "C" to move to informed consent
        if (Input.GetKeyDown(KeyCode.C) && block == "0.PleaseWait")
        {
            SceneManager.LoadScene(2);
        }

        //quits the game if the subject ever presses the ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //advances to subject registration (id/condition input)
    //these are triggered by buttons on that page, rather than events
    public void GoToConsent()
    {
        //if input in registration = P; go to parent informed consent 
        if (CombinedData.CONDITION == "p" | CombinedData.CONDITION == "P")
        {
            SceneManager.LoadScene(1);
        }
        //if input in registration = N; go to nonparent informed consent
        else if (CombinedData.CONDITION == "n" | CombinedData.CONDITION == "N")
        {
            SceneManager.LoadScene(8); //may need to go back and change # later
        }
        
    }

    //advances to gameplay (narrative)
    public void GoToTestButton()
    {
        SceneManager.LoadScene(3);
    }

    //advances to baby trials instructions
    public void GoToBabyInstructions()
    {
        SceneManager.LoadScene(5);
    }

    //advances to end of game instructions
    public void GoToEndGame()
    {
        SceneManager.LoadScene(7);
    }

    //advances to instructions before the last neutral scene
    public void GoToFinalInstructions()
    {
        SceneManager.LoadScene(9);
    }


    //calls on currently active scene
    public void CurrentBlock()
    {
        Scene blockstage = SceneManager.GetActiveScene();
        block =  blockstage.name;
    }
}

