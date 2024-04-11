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
        //quits the game if the subject ever presses the ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            SceneManager.LoadScene(4);
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SceneManager.LoadScene(5);
        }
        if (Input.GetKeyDown(KeyCode.F6))
        {
            SceneManager.LoadScene(6);
        }
        if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene(7);
        }
    }

    //calls on currently active scene
    public void CurrentBlock()
    {
        Scene blockstage = SceneManager.GetActiveScene();
        block = blockstage.name;
    }
}

