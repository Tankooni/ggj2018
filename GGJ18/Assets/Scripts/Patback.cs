using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Patback : MonoBehaviour {

	public bool ObjectIsWelded { get; set; }

	public void Awake() {
		ObjectGenerator.Init();
		ObjectIsWelded = true;
	}

	private void OnTriggerStay(Collider collider)
    {
		var grabbingObject = collider.gameObject.GetComponent<VRTK_InteractGrab>()
			?? collider.GetComponentInParent<VRTK_InteractGrab>();

		if(canGrab(grabbingObject) && ObjectIsWelded) {
			ObjectIsWelded = false;
			
			var obj = ObjectGenerator.CreateRandomObject(.8f, 1.2f);
			grabbingObject.GetComponent<VRTK_InteractTouch>().ForceTouch(obj);
			grabbingObject.AttemptGrab();

			ObjectIsWelded = false;
		}
	}

	private bool canGrab(VRTK_InteractGrab grabbingObject) {
		return (grabbingObject && grabbingObject.GetGrabbedObject() == null
						&& grabbingObject.IsGrabButtonPressed());
	}
}
