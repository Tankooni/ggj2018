using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WeldHand : MonoBehaviour {

    private VRTK_InteractGrab interactGrab;
    private VRTK_InteractUse interactUse;
    private VRTK_ControllerEvents controllerEvents;

	private AudioSource track1;
	private AudioSource track2;

	private bool isGrowing = false;
    private bool hasGrown = false;
    private float growValue = 1.1f;
    private float growTotal = 0;

	private void Awake()
    {
        this.interactGrab = this.gameObject.GetComponent<VRTK_InteractGrab>();
		controllerEvents = gameObject.GetComponent<VRTK_ControllerEvents>();
        //interactUse = gameObject.GetComponent<VRTK_InteractUse>();


        interactGrab.ControllerUngrabInteractableObject += OnControllerUngrabInteractableObject;
        interactGrab.ControllerGrabInteractableObject += OnControllerGrabInteractableObject;
        controllerEvents.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.GripPress, true, OnControllerGripPress);

		track1 = GameObject.Find("Track1").GetComponent<AudioSource>();
		track2 = GameObject.Find("Track2").GetComponent<AudioSource>();
		//interactUse.UseButtonPressed += OnControllerGripPress;
	}

    private void Start()
    {
        
    }

    private void Update()
    {
        if(isGrowing)
        {
			track1.volume = Mathf.Lerp(track1.volume, 0, -.1f);
			track2.volume = Mathf.Lerp(track2.volume, 1, .1f);
			var weldy = ObjectGenerator.WeldedObjects.transform;
            weldy.SetGlobalScale(new Vector3(weldy.lossyScale.x * growValue, weldy.lossyScale.y * growValue, weldy.lossyScale.z * growValue));
            growTotal += growValue - 1;
            isGrowing = !(growTotal >= 3);
            hasGrown = !isGrowing;
            //Vector3.Slerp()
        }
    }

    private void OnControllerGrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        var weldableObject = e.target.GetComponent<WeldableObject>();

        if (weldableObject != null)
            weldableObject.OnGrabbed();
    }

	private void OnControllerUngrabInteractableObject(object sender, ObjectInteractEventArgs e)
	{
		var weldableObject = e.target.GetComponent<WeldableObject>();

		if (weldableObject != null)
			weldableObject.OnUngrabbed();
	}

	private void OnControllerGripPress(object sender, ControllerInteractionEventArgs e)
    {
		Debug.Log("A gripping tale");
        if(!hasGrown)
            isGrowing = true;
	}
}
