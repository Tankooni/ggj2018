using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using VRTK;

public static class ObjectGenerator {

	private static GameObject[] constructables = 
		Resources.LoadAll("Assets/Prefabs/Constructables", typeof(GameObject)).Cast<GameObject>() as GameObject[];
	private static System.Random rng = new System.Random();

  public static GameObject CreateRandomObject(float minScale, float maxScale)
  {
		List<GameObject> objects = new List<GameObject>();
		foreach(var o in constructables) {
			Debug.Log (o);
			if(o is GameObject) {
				objects.Add((GameObject) o);
			}
		}
		var i = rng.Next(0, objects.Count);
		var obj = MonoBehaviour.Instantiate(objects[i], new Vector3(0,0,0), Quaternion.identity);
		
		float scale = (float)((rng.NextDouble() % (maxScale - minScale)) + minScale);
		obj.transform.localScale.Scale(new Vector3(scale, scale, scale));

		var interactTouch = obj.AddComponent<VRTK_InteractTouch>();

		Debug.Log (obj);
		return obj;
  }
}
