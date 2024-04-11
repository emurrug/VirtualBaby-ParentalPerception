using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BabyWander : MonoBehaviour
{
    public int xPos;
    public int zPos;

    public GameObject theDestination;

    public GameObject toy1;
    public GameObject toy2;
    public GameObject toy3;
    //NavMeshAgent theBaby;
    public GameObject baby;
    public float speed;
    public float rotateSpeed;

   
    void Start()
    {
        //theBaby = GetComponent<NavMeshAgent>();
        speed = 0.5f;
        rotateSpeed = 1.5f;
    }

        private void Update()
    {
        if (theDestination.transform.position == toy1.transform.position |
            theDestination.transform.position == toy2.transform.position |
            theDestination.transform.position == toy3.transform.position)
        {
            StartCoroutine("SetRandomLocation");
        }

        //theBaby.SetDestination(theDestination.transform.position);
        baby.transform.position = Vector3.MoveTowards(baby.transform.position, theDestination.transform.position, speed * Time.deltaTime);

        //set up a rotate towards
        Vector3 targetDirection = theDestination.transform.position - baby.transform.position;
        float singleStep = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        baby.transform.rotation = Quaternion.LookRotation(newDirection);


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "baby")
        {
            StartCoroutine("SetRandomLocation");
        }
    }

    IEnumerator SetRandomLocation()
    {
        xPos = Random.Range(249, 262);
        zPos = Random.Range(182, 190);
        theDestination.transform.position = new Vector3(xPos, 0, zPos);
        yield return null;
    }
}