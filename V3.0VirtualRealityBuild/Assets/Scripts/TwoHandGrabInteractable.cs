using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;

    public Vector3 selectingVector;
    public Vector3 secondVector;

    public GameObject LeftHand;
    public GameObject RightHand;
    
    public bool LeftHandTouch = false;
    public bool RightHandTouch = false;
    public bool TwoHandTouch = false;
    //these is a similar object, but for the purpose of EmptyWorldTutorial.cs to access grab info
    public bool TwoHandGrab = false;

    public XRRayInteractor RightXRRayInteractor;
    public XRRayInteractor LeftXRRayInteractor;

    public Rigidbody OwnRigidBody;

    //Im going to get a y positon for the ground and use this to determine when the object has returned back to the floor
    //for kinematics physics functions ^
    public int yPos;
    public int newYpos;



    //for this script to work, several things must first be in place within Unity: 
    //1. there are 2 hand controllers, each with a direct interactor (I am actually using a Ray Interactor though)
    //      -the hand model has a rigidbody (gravity & kinematic) and collider (non-trigger)
    //2. The picked up object must have a collider (non-trigger) and rigidbody (gravity, non-kinematic)
    //3. There are attachment points that can be used to grab the pickup object 
    //      -these have colliders (nontriggers), simple interactables (with self in collider list), and rigidbodyies (gravity only)
    //      that are on a different layer than both the default and the picked up object. it is best to set these
    //      as only interactable with the default and no other layer
    //      -the attachment point does not have a rigidbody, but the secondhands do
    //4. the pick up object has this script with the following settings preferred: 
    //      -itself as the listed collider (no others)
    //      -instantanous movement type
    //      -attach transform object(s) is the attachment point(s) (set only to one side of the object body such as R for R-handers)
    //          (this wont matter later as they will have to be a two-hand touch to activate either way)
    //      -second hand grab point(s) (set to other side of object body)
    //          (this wont lock in an attachment transform, but it will modify the rotational vector)

    void Start()
    {
       //checks the list of second hand objects and adds a listener to determin if it is being selected
        foreach (var item in secondHandGrabPoints)
        {
            item.onSelectEntered.AddListener(OnSecondHandGrab);
            item.onSelectExited.AddListener(OnSecondHandRelease);
        }
        yPos = Mathf.RoundToInt(this.transform.position.y);

//new plan of attack: 
//turning on and off the ray interactors doesn't seem to work so well
//
        RightXRRayInteractor.enableInteractions = false;
        LeftXRRayInteractor.enableInteractions = false;

    }

    //checks to see if object is being touched by one or both of the hands
    //(how I got around the direct interactor not working properly /s)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "LeftHand")
        {
            LeftHandTouch = true;

        }
        if (collision.gameObject.name == "RightHand")
        {
            RightHandTouch = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.name == "LeftHand")
        {
            LeftHandTouch = false;
        }
        if (collision.gameObject.name == "RightHand")
        {
            RightHandTouch = false;
        }
    }

   void Update()
   {
        if (Input.GetButton("XRI_Right_TriggerButton") == false || Input.GetButton("XRI_Left_TriggerButton") == false)
        {
            RightXRRayInteractor.enableInteractions = false;
            LeftXRRayInteractor.enableInteractions = false;
            TwoHandGrab = false;
        }
        if (RightHandTouch == true && LeftHandTouch == true)
        {
            TwoHandTouch = true;//this is for other scripts to access
            if (Input.GetButton("XRI_Right_TriggerButton") == true && Input.GetButton("XRI_Left_TriggerButton") == true)
            {
                RightXRRayInteractor.enableInteractions = true;
                LeftXRRayInteractor.enableInteractions = true;
                //should also turn off kinematics until on the ground again
                OwnRigidBody.isKinematic = false;
                TwoHandGrab = true;
                GetComponent<Animator>().enabled = false;
            }
        }
        if (RightHandTouch == false || LeftHandTouch == false)
        {
            TwoHandTouch = false; //this is for other scripts to access
            //turns off the kinematics when the ball has touched the ground again
            //still want to change it so that it looks like it rolls a bit once it touches the ground
            newYpos = Mathf.RoundToInt(this.transform.position.y); 
            
            if (yPos == newYpos && 
                Mathf.RoundToInt(OwnRigidBody.velocity.x) <.01 && 
                Mathf.RoundToInt(OwnRigidBody.velocity.z) < .01)
                {
                OwnRigidBody.isKinematic = true;
                GetComponent<Animator>().enabled = true;
            }
        }
   }


    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if(secondInteractor && selectingInteractor)
        {
            //IMPORTANT: when designing the attach points for the picked up object, the Z axis must being pointed in the correct direction!
            //with the current setup, that means the z axis will be facing inward (palm direction) towards the center of the object mass
            selectingInteractor.attachTransform.rotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up);
        }

        base.ProcessInteractable(updatePhase);
    }

    //does nothing to the location of the object; only registers if a secondhand point is being selected and grabbed/released
    public void OnSecondHandGrab(XRBaseInteractor interactor)
    {
        secondInteractor= interactor;
        attachInitialRotation = interactor.attachTransform.localRotation;
    }
    public void OnSecondHandRelease(XRBaseInteractor interactor)
    {
        secondInteractor = null;
        interactor.attachTransform.localRotation = attachInitialRotation;
    }

    //prevents object from snapping to the other hand when it is selected by already in one hand
    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && !isalreadygrabbed;
    }
}
