using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WeldHand : MonoBehaviour {

    private VRTK_InteractGrab interactGrab;

	private void Awake()
    {
        this.interactGrab = this.gameObject.GetComponent<VRTK_InteractGrab>();
        interactGrab.ControllerUngrabInteractableObject += OnControllerUngrabInteractableObject;
        interactGrab.ControllerGrabInteractableObject += OnControllerGrabInteractableObject;
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
}
