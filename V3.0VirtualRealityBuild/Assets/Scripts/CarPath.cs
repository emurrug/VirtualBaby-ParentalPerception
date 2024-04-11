using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarPath : MonoBehaviour
{
    public GameObject car;
    public GameObject[] pathPoints;
    public int numberOfPoints;
    public int speed;
    public float rotateSpeed;
    public int waittime;

    private Vector3 actualPosition;
    public int x;

    public GameObject originpoint;
    public GameObject endpoint;
    public GameObject carskin;

    // A note about car speeds: Each unit (the default unity grid) corresponds approximately to 1 ft. The distance
    //between origin and destination is 258.81 ft. This means when "speed = 258", this means the car reaches its destination in ~1sec.
    //The speeds below correspond to ft/s conversions (rounded) of 30, 40, 50, 60, and 70 mph.
    public List<int> speedslist1 = new List<int>() { 44, 59, 73, 88, 103, 44, 59, 73, 88, 103 };
    public List<int> speedslist2 = new List<int>() { }; //empty list to move all of speedslist1 into so speeds aren't repeated (i.e., randomization without replacement)

    public static int scoreValue = 0; //how many times the player correctly hit SPACE to wave down cars
    public bool AlreadyScored = false; //prevents subs from spamming SPACE more than once per trial
    public int trial = 1; //increases each time the car loops back over (includes both "misses" and "hits")

    public RaiseHand raisehand;

    public GameObject ping; 
    // Start is called before the first frame update
    void Start()
    {
        //resets the car score counter so that tutorial doesn't count against
        if (SceneManager.GetActiveScene().name == "2.NoBabyScene")
        {
            scoreValue = 0;
        }
        if (SceneManager.GetActiveScene().name == "3.BabyInstructions")
        {
            scoreValue = 5;
        }

        x = 0;
        rotateSpeed = 5;

        carskin = car.transform.Find("sedan").gameObject;

        ping.active = false;
    }

    // Update is called once per frame
    void MoveTowards()
    {
        car.transform.position = Vector3.MoveTowards(actualPosition, pathPoints[x].transform.position, speed * Time.deltaTime);

        //set up a rotate towards action
        Vector3 targetDirection = pathPoints[x].transform.position - car.transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        car.transform.rotation = Quaternion.LookRotation(newDirection);
        car.transform.rotation = Quaternion.LookRotation(newDirection);
    }


    void Update()
    {
        if (actualPosition != pathPoints[x].transform.position)
        {
            actualPosition = car.transform.position;
            MoveTowards();
        }

        //adds a point to score if they wave while the car is moving
        if (raisehand.Rhigher || raisehand.Lhigher)
        {
            if (x != 0 &&
            x != 10 &&
            AlreadyScored == false)
            {
                scoreValue += 1;
                AlreadyScored = true; //prevents re-scoring
                StartCoroutine("Ping");
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "path")
        {
            CheckX();
        }
    }


    void CheckX()
    {
        if(x != 0 && x != 1 && x != 10)
        {
            x++;
        }
        else if(x == 0)
        {
            StartCoroutine("RandomStart");
        }
        else if (x == 1)
        {
            StartCoroutine("StopSign");
        }
        if (x >= 10)
        {
            StartCoroutine("RestartCar");
        }
    }

    IEnumerator RandomStart()
    {

        speed = speedslist1[Random.Range(0, (speedslist1.Count))];
        AlreadyScored = false;
        carskin.active = true;
        waittime = Random.Range(5,15);
        yield return new WaitForSeconds(waittime);
        x++;
    }
    IEnumerator StopSign()
    {
        yield return new WaitForSeconds(3);
        x++;
    }
    IEnumerator RestartCar()
    {
        //identifies the current speed and removes it from the current speedslist
        
        if (AlreadyScored == true)
        {
            int speedslist1index = speedslist1.IndexOf(speed);
            int temp1 = speedslist1[speedslist1index];
            speedslist2.Add(temp1);
            speedslist1.RemoveAt(speedslist1index);
        }
        x = 0;
        carskin.active = false;
        yield return null;
    }
    IEnumerator Ping()
    {
        ping.active = true;
        yield return new WaitForSeconds(1);
        ping.active = false;
    }

}
