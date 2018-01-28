using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

public static class ObjectGenerator {

	private static GameObject[] constructables;
	private static System.Random rng = new System.Random();

	public static void Init() {
		var constructables_obj  = Resources.LoadAll("Constructables").Cast<GameObject>().ToArray();
        Debug.Log(constructables_obj.Count());
        constructables = constructables_obj;
	}

  public static GameObject CreateRandomObject(float minScale, float maxScale)
  {
		var i = rng.Next(0, constructables.Length);
		var obj = MonoBehaviour.Instantiate(constructables[i], new Vector3(0,0,0), Quaternion.identity);
		
		float scale = (float)((rng.NextDouble() % (maxScale - minScale)) + minScale);
		obj.transform.SetGlobalScale(new Vector3(scale, scale, scale));

        obj.AddComponent<BoxCollider>();

        var rbody = obj.AddComponent<Rigidbody>();
        rbody.constraints = RigidbodyConstraints.None;
        rbody.useGravity = true;
        rbody.isKinematic = false;

        var int_obj = obj.AddComponent<VRTK_InteractableObject>();
		int_obj.isGrabbable = true;
		int_obj.validDrop = VRTK_InteractableObject.ValidDropTypes.DropAnywhere;
        int_obj.holdButtonToUse = false;
        int_obj.validDrop = VRTK_InteractableObject.ValidDropTypes.NoDrop;

		obj.AddComponent<WeldableObject>();

		Debug.Log(obj);
		return obj;
  }
}
