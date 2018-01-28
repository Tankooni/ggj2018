using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Patback : MonoBehaviour {

	private bool ObjectPlaced {get; set;}
	public void Awake() {
		ObjectGenerator.Init();
		ObjectPlaced = true;
	}

	private void OnTriggerStay(Collider collider)	{
		var grabbingObject = collider.gameObject.GetComponent<VRTK_InteractGrab>()
			?? collider.GetComponentInParent<VRTK_InteractGrab>();

		if(canGrab(grabbingObject) && ObjectPlaced) {
			ObjectPlaced = false;
			
			var obj = ObjectGenerator.CreateRandomObject(.1f, .5f);
			grabbingObject.GetComponent<VRTK_InteractTouch>().ForceTouch(obj);
			grabbingObject.AttemptGrab();
		}
	}

	private bool canGrab(VRTK_InteractGrab grabbingObject) {
		return (grabbingObject && grabbingObject.GetGrabbedObject() == null
						&& grabbingObject.IsGrabButtonPressed());
	}
}
