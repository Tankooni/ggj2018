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
        e.target.AddComponent<WeldableObject>();
    }

    private void OnControllerUngrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        Destroy(e.target.GetComponent<WeldableObject>());
    }
}
