using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class HammerHand : MonoBehaviour {

	public bool usePlayerScale = true;

	[Tooltip("The VRTK Body Physics script to use for dealing with climbing and falling. If this is left blank then the script will need to be applied to the same GameObject.")]
	public VRTK_BodyPhysics bodyPhysics;

	private Rigidbody myRigidbody;
	VRTK_ControllerReference controllerReference;

	protected Transform playArea;

	private void Awake()
	{
		myRigidbody = GetComponent<Rigidbody>();
		//bodyPhysics = (bodyPhysics != null ? bodyPhysics : GetComponentInChildren<VRTK_BodyPhysics>());
	}

	protected virtual void OnEnable()
	{
		playArea = VRTK_DeviceFinder.PlayAreaTransform();
		Debug.Log(playArea);
		bodyPhysics = (bodyPhysics != null ? bodyPhysics : playArea.GetComponentInChildren<VRTK_BodyPhysics>());
		Debug.Log(bodyPhysics);
		controllerReference = VRTK_ControllerReference.GetControllerReference(this.transform.parent.gameObject);
		Debug.Log(controllerReference);
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

		Vector3 velocity = Vector3.zero;

		if (VRTK_ControllerReference.IsValid(controllerReference))
		{
			velocity = -VRTK_DeviceFinder.GetControllerVelocity(controllerReference);
			if (usePlayerScale)
			{
				velocity = playArea.TransformVector(-velocity);
			}
			else
			{
				velocity = playArea.TransformDirection(-velocity);
			}
		}

		bodyPhysics.ApplyBodyVelocity(velocity, true, true);

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
