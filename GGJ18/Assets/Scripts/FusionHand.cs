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

		var interactUse = this.gameObject.GetComponent<VRTK_InteractUse> ();
		interactUse.UseButtonPressed += SpawnObject;
	}

    private void ChangeGrabbableColor(object sender, ObjectInteractEventArgs e)
    {
        e.target.GetComponent<Rigidbody>().useGravity = false;
    }

	private void SpawnObject(object sender, VRTK.ControllerInteractionEventArgs e) {
		Debug.Log ("Spawning object");
		var obj = ObjectGenerator.CreateRandomObject (2f, 5f);
		obj.transform.position = transform.position;
	}
}
