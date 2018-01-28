using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class WeldableObject : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Color originalColor;
    private int weldablesTouchingCount;

    private VRTK_InteractableObject interactableObject;
    private new Rigidbody rrigidbody;

    public bool IsGrabbed
    {
        get
        {
            if (this.interactableObject == null)
                return false;
            else
                return this.interactableObject.IsGrabbed();
        }
    }

    public bool IsWelded
    {
        get
        {
            if (this.rrigidbody == null)
                return true;
            else
                return this.rrigidbody.isKinematic;
        }
    }

    private bool IsTouchingWeldable
    {
        get
        {
            return weldablesTouchingCount > 0;
        }
    }

    private void Awake()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.interactableObject = this.GetComponent<VRTK_InteractableObject>();
        this.rrigidbody = this.GetComponent<Rigidbody>();

        this.originalColor = meshRenderer.material.color;
    }

    public void OnGrabbed()
    {

    }

    public void OnUngrabbed()
    {
        //if (!IsTouchingWeldable)
        //    return;

        //this.rrigidbody.isKinematic = true;
        //this.interactableObject.isGrabbable = false;

        // Disable physics
        rrigidbody.isKinematic = true;
        // Change the grab type
        // Make a climbable one with default settings (for now)
        var climbableGrabAttach = this.gameObject.AddComponent<VRTK.GrabAttachMechanics.VRTK_ClimbableGrabAttach>();
        // Destroy the old one
        Destroy(this.interactableObject.grabAttachMechanicScript);
        // Replace it with the new one
        this.interactableObject.grabAttachMechanicScript = climbableGrabAttach;

    }

    private void Update()
    {
        SetHighlightColor();
    }

    private void SetHighlightColor()
    {
        if (IsGrabbed) // && this.IsTouchingWeldable)
            this.meshRenderer.material.color = Color.yellow;
        else
            this.meshRenderer.material.color = originalColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherWeldableObject = collision.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject != null && !otherWeldableObject.IsGrabbed && otherWeldableObject.IsWelded)
            this.weldablesTouchingCount++;
    }

    private void OnCollisionExit(Collision collision)
    {
        var otherWeldableObject = collision.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject != null && !otherWeldableObject.IsGrabbed && otherWeldableObject.IsWelded)
            this.weldablesTouchingCount = Mathf.Max(0, weldablesTouchingCount - 1);
    }
}
