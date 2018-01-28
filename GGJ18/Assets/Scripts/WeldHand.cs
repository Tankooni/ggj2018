using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WeldHand : MonoBehaviour {

    private VRTK_InteractGrab interactGrab;
	private VRTK_ControllerEvents controllerEvents;

	private bool hasGrown = false;

	private void Awake()
    {
        this.interactGrab = this.gameObject.GetComponent<VRTK_InteractGrab>();
		controllerEvents = gameObject.GetComponent<VRTK_ControllerEvents>();

		interactGrab.ControllerUngrabInteractableObject += OnControllerUngrabInteractableObject;
        interactGrab.ControllerGrabInteractableObject += OnControllerGrabInteractableObject;

		controllerEvents.SubscribeToButtonAliasEvent(VRTK_ControllerEvents.ButtonAlias.GripClick, true, OnControllerGripPress);

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
		hasGrown = true;
		foreach(Transform childGrowObject in ObjectGenerator.WeldedObjects.transform)
		{
			childGrowObject.localScale.Scale(new Vector3(3f, 3f, 3f));
		}
	}
}
