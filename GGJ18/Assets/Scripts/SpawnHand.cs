using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class SpawnHand : MonoBehaviour {

	private void Awake() {
		var interactUse = this.gameObject.GetComponent<VRTK_InteractUse> ();
		interactUse.UseButtonPressed += SpawnObject;
	}

	private void SpawnObject(object sender, VRTK.ControllerInteractionEventArgs e) {
		Debug.Log ("Spawning object");
		var obj = ObjectGenerator.CreateRandomObject (2f, 5f);
		obj.transform.position = transform.position;
	}
}
