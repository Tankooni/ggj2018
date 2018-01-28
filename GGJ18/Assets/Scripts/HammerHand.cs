using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerHand : MonoBehaviour {

	private Rigidbody myRigidbody;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
	}

	// Use this for initialization
	private void Start ()
	{
		
	}

	// Update is called once per frame
	private void Update ()
	{
		
	}

	private void OnCollisionEnter(Collision c)
	{
		Debug.Log("Col Enter");

		var foreignBody = c.gameObject.GetComponent<Rigidbody>();
		if(foreignBody != null)
		{
			Debug.Log("Adding force from hammer");

			foreignBody.AddForce(myRigidbody.velocity, ForceMode.VelocityChange);
		}
	}

	private void OnCollisionStay(Collision c)
	{
		Debug.Log("Col Stay");
	}

	private void OnCollisionExit(Collision c)
	{
		Debug.Log("Col Exit");
	}
}
