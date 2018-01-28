using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHand : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter(Collision c)
	{
		Debug.Log("Col Enter");
	}

	void OnCollisionStay(Collision c)
	{
		Debug.Log("Col Stay");
	}

	void OnCollisionExit(Collision c)
	{
		Debug.Log("Col Exit");
	}
}
