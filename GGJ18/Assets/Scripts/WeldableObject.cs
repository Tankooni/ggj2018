﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject), typeof(Rigidbody), typeof(Collider))]
[RequireComponent(typeof(VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter))]
public class WeldableObject : MonoBehaviour
{
    private static Color CLIMBABLE_COLOR = Color.yellow;
    private static Color TOUCHING_WELDABLE_COLOR = Color.green;
    private static Color TOUCHING_COLOR = Color.cyan;

    private MeshRenderer meshRenderer;
    private Material originalMaterial;
    private Color originalColor;
    private int weldablesTouchingCount;
    private bool isWelded = false;

    private VRTK_InteractableObject interactableObject;
    private VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter outlineHighlighter;
    private Rigidbody rrigidbody;
    private Collider ccollider;

    public bool StartWelded = false;

    public bool IsWelded {
        get { return this.isWelded; }
        set { this.isWelded = value; }
    }

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

    private bool IsTouchingWeldable
    {
        get
        {
            return weldablesTouchingCount > 0;
        }
    }

    private void LateUpdate()
    {
        if (IsWelded)
        {
            // Keep turning isKinematic on because SOMEONE keeps turning it off!
            if(this.rrigidbody != null) {
                this.rrigidbody.isKinematic = true;
                this.meshRenderer.material.color = originalColor;
            }
        }
        else if (IsGrabbed)
        {
            if (IsTouchingWeldable)
            {
                this.interactableObject.touchHighlightColor = TOUCHING_WELDABLE_COLOR;
                this.meshRenderer.material.color = TOUCHING_WELDABLE_COLOR;
            }
            else
            {
                this.interactableObject.touchHighlightColor = TOUCHING_COLOR;
                this.meshRenderer.material.color = Color.white;
            }
        }     
    }

    private void Awake()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.interactableObject = this.GetComponent<VRTK_InteractableObject>();
        this.rrigidbody = this.GetComponent<Rigidbody>();
        this.outlineHighlighter = this.GetComponent<VRTK.Highlighters.VRTK_OutlineObjectCopyHighlighter>();
        this.ccollider = this.GetComponent<Collider>();

        this.originalMaterial = meshRenderer.material;
        this.originalColor = meshRenderer.sharedMaterial.color;

        // Initialize IsWelded
        this.IsWelded = this.StartWelded;

        // If not welded at start then give unwelded material
        if (!IsWelded)
        {
            this.meshRenderer.material = Resources.Load("Materials/DepressingMaterial") as Material;
            this.interactableObject.touchHighlightColor = TOUCHING_COLOR;
        }
    }

    public void OnGrabbed()
    {
        this.ccollider.isTrigger = true;
    }

    public void OnUngrabbed()
    {
        if (!IsWelded && IsTouchingWeldable)
        {            
            //// Destroy the rigidbody since we'll never need it again
            //Destroy(rrigidbody);
            //// Clear the reference so we don't get any problems later
            //this.rrigidbody = null;

            // --- Change the grab type ---
            // Make a climbable one with default settings (for now)
            var climbableGrabAttach = this.gameObject.AddComponent<VRTK.GrabAttachMechanics.VRTK_ClimbableGrabAttach>();
            // Destroy the old one
            Destroy(this.interactableObject.grabAttachMechanicScript);
            // Replace it with the new one
            this.interactableObject.grabAttachMechanicScript = climbableGrabAttach;

            // Change the highlighter color
            this.interactableObject.touchHighlightColor = CLIMBABLE_COLOR;

            // Restore the object's original materiala
            this.meshRenderer.material = originalMaterial;

            // MARK IT AS WELDED
            this.IsWelded = true;

            Debug.Log("IT WELDED!");
        }

        this.ccollider.isTrigger = false;

        Debug.Log("IT REFUSED TO WELD! " + IsWelded + " " + IsTouchingWeldable);
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherWeldableObject = other.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject != null && !otherWeldableObject.IsGrabbed && otherWeldableObject.IsWelded)
            this.weldablesTouchingCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        var otherWeldableObject = other.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject != null && !otherWeldableObject.IsGrabbed && otherWeldableObject.IsWelded)
            this.weldablesTouchingCount = Mathf.Max(0, weldablesTouchingCount - 1);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        
    //}

    //private void OnCollisionExit(Collision collision)
    //{
        
    //}
}
