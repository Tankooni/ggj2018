﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Patback : MonoBehaviour {

	private bool ObjectPlaced {get; set;}
	public void Awake() {
		ObjectGenerator.Init();
	}

	private void OnTriggerStay(Collider collider)	{
		//Debug.Log("Patback ready for AcTioN!");
		var grabbingObject = collider.gameObject.GetComponent<VRTK_InteractGrab>()
			?? collider.GetComponentInParent<VRTK_InteractGrab>();

		if(canGrab(grabbingObject) && !ObjectPlaced) {
			Debug.Log("Grabbing object");

			var obj = ObjectGenerator.CreateRandomObject(2f, 3f);
			grabbingObject.GetComponent<VRTK_InteractTouch>().ForceTouch(obj);
			grabbingObject.AttemptGrab();

			ObjectPlaced = false;
		}
	}

	private bool canGrab(VRTK_InteractGrab grabbingObject) {
		return (grabbingObject && grabbingObject.GetGrabbedObject() == null
						&& grabbingObject.IsGrabButtonPressed());
	}
}
