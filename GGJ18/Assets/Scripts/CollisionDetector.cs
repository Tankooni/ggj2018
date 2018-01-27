using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }

    private void OnCollisionExit(Collision collision)
    {
        collision.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
