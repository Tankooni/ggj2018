using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class FusionHand : MonoBehaviour {

    private VRTK_InteractGrab interactGrab;

	private void Awake()
    {
        this.interactGrab = this.gameObject.GetComponent<VRTK_InteractGrab>();
        interactGrab.ControllerUngrabInteractableObject += ChangeGrabbableColor;
	}

    private void ChangeGrabbableColor(object sender, ObjectInteractEventArgs e)
    {
        e.target.GetComponent<Rigidbody>().useGravity = false;
    }
}
