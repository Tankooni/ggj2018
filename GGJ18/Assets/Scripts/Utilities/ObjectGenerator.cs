using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

public static class ObjectGenerator {

	private static GameObject[] constructables;
	private static System.Random rng = new System.Random();

	public static void Init() {
		constructables  = Resources.LoadAll("Constructables", typeof(GameObject)).Cast<GameObject>() as GameObject[];
	}

  public static GameObject CreateRandomObject(float minScale, float maxScale)
  {
		var i = rng.Next(0, constructables.Length);
		var obj = MonoBehaviour.Instantiate(constructables[i], new Vector3(0,0,0), Quaternion.identity);
		
		float scale = (float)((rng.NextDouble() % (maxScale - minScale)) + minScale);
		obj.transform.localScale.Scale(new Vector3(scale, scale, scale));

		var interactTouch = obj.AddComponent<VRTK_InteractTouch>();

		Debug.Log(obj);
		return obj;
  }
}
