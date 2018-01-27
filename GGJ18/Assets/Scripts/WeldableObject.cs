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

    public bool IsGrabbed { get { return interactableObject.IsGrabbed(); } }

    private bool IsTouchingWeldable
    {
        get
        {
            return weldablesTouchingCount > 0;
        }
    }

    private void Awake()
    {
        this.meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        this.originalColor = meshRenderer.material.color;

        this.interactableObject.InteractableObjectUngrabbed += OnInteractableObjectUngrabbed;
    }

    private void OnInteractableObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Update()
    {
        SetHighlightColor();
    }

    private void SetHighlightColor()
    {
        if (this.IsTouchingWeldable && IsGrabbed)
            this.meshRenderer.material.color = Color.yellow;
        else
            this.meshRenderer.material.color = originalColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var otherWeldableObject = collision.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject == null || otherWeldableObject.IsGrabbed)
            return;

        this.weldablesTouchingCount++;
    }

    private void OnCollisionExit(Collision collision)
    {
        var otherWeldableObject = collision.gameObject.GetComponent<WeldableObject>();

        if (otherWeldableObject == null || otherWeldableObject.IsGrabbed)
            return;

        this.weldablesTouchingCount = Mathf.Max(0, weldablesTouchingCount - 1);
    }
}
