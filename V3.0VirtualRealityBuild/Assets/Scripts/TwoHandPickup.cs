using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TwoHandPickup : MonoBehaviour
{
    public GameObject BabyCollider;

    [SerializeField] private Animator babyanimationcontrol;
    Rigidbody ColliderBody;
    Rigidbody ColliderRightHand;
    Rigidbody ColliderLeftHand;

    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject BabyHeldPosition;
    public GameObject LeftPickupPoint;
    public GameObject RightPickupPoint;

    public bool LeftHandTouch = false; 
    public bool RightHandTouch = false;

    public bool HoldingBaby = false;



    void Start()
    {
        babyanimationcontrol.SetInteger("PickBabyUp", 0);
        ColliderBody = BabyCollider.GetComponent<Rigidbody>();
        ColliderRightHand = RightHand.GetComponent<Rigidbody>();
        ColliderLeftHand = LeftHand.GetComponent<Rigidbody>();
    }


    void Update()
    {

        if (Input.GetButton("XRI_Right_TriggerButton") == true &&
            Input.GetButton("XRI_Left_TriggerButton") == true)
        {
            StartCoroutine("CheckForTouch");
        }
        else if (Input.GetButton("XRI_Right_TriggerButton") == false ||
                 Input.GetButton("XRI_Left_TriggerButton") == false)
        {
            HoldingBaby = false;
        }

        if (HoldingBaby == true)
        {
            BabyCollider.transform.position = new Vector3(
            BabyHeldPosition.transform.position.x,
            BabyHeldPosition.transform.position.y,
            BabyHeldPosition.transform.position.z);

            //for the above to work, the held position must be in the exact spot as the
            //baby model's center collision (right now the Nav Mesh point)

            //one very sloppy solution might be just to constrain how participants hold their hands...

            //ColliderBody.constraints = RigidbodyConstraints.FreezeRotation;
            //ColliderLeftHand.constraints = RigidbodyConstraints.FreezeRotationX;
            //ColliderRightHand.constraints = RigidbodyConstraints.FreezeRotationX;



        }
        else if (HoldingBaby == false)
        {
            babyanimationcontrol.SetInteger("PickBabyUp", 0);
            ColliderBody.constraints = RigidbodyConstraints.None;
        }


    }

    void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.name == "LeftHand")
        {
            StartCoroutine("LeftBoop");
        }
        else if (collision.gameObject.name == "RightHand")
        {
            StartCoroutine("RightBoop");
        }
        
    }
    IEnumerator LeftBoop()
    {   
        LeftHandTouch = true;
        //Debug.Log("Left boop");
        yield return new WaitForSeconds(1);
        LeftHandTouch = false;
    }
    IEnumerator RightBoop()
    {
        RightHandTouch = true;
        //Debug.Log("Right boop");
        yield return new WaitForSeconds(1);
        RightHandTouch = false;
    }

    IEnumerator CheckForTouch()
    {
        if (RightHandTouch == true && LeftHandTouch == true)
        {
            babyanimationcontrol.SetInteger("PickBabyUp", 1);
            HoldingBaby = true;
            yield return null;
        }
    }


}
