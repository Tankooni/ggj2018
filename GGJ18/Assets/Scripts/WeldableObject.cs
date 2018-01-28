using UnityEngine;
using VRTK;
using VRTK.Highlighters;
using VRTK.GrabAttachMechanics;
using System.Collections.Generic;

[RequireComponent(typeof(VRTK_InteractableObject))]
[RequireComponent(typeof(VRTK_OutlineObjectCopyHighlighter))]
//[RequireComponent(typeof(Collider))]
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
    private VRTK_OutlineObjectCopyHighlighter outlineHighlighter;
    private Rigidbody rrigidbody;

    private List<Collider> colliders;

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

    private void SetColliderTrigger(bool isTrigger)
    {
        foreach (var col in this.colliders)
        {
            col.isTrigger = isTrigger;
        }
    }

    private void LateUpdate()
    {
        if (IsWelded)
        {
            this.meshRenderer.material.color = originalColor;
        }
        else if (IsGrabbed)
        {
            if (IsTouchingWeldable)
            {
                this.interactableObject.touchHighlightColor = TOUCHING_WELDABLE_COLOR;
                this.meshRenderer.material.color = TOUCHING_WELDABLE_COLOR;
                interactableObject.validDrop = VRTK_InteractableObject.ValidDropTypes.DropAnywhere;
            }
            else
            {
                this.interactableObject.touchHighlightColor = TOUCHING_COLOR;
                this.meshRenderer.material.color = Color.white;
                interactableObject.validDrop = VRTK_InteractableObject.ValidDropTypes.NoDrop;
            }
        }     
    }

    private void Awake()
    {
        this.meshRenderer = this.GetComponent<MeshRenderer>();
        this.interactableObject = this.GetComponent<VRTK_InteractableObject>();
        this.rrigidbody = this.GetComponent<Rigidbody>();
        this.outlineHighlighter = this.GetComponent<VRTK_OutlineObjectCopyHighlighter>();

        this.colliders = new List<Collider>();
        this.colliders.AddRange(this.GetComponents<Collider>());
        this.colliders.AddRange(this.GetComponentsInChildren<Collider>());

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
        if (!IsWelded)
            SetColliderTrigger(true);
    }

    public void OnUngrabbed()
    {
        if (!IsWelded && IsTouchingWeldable)
        {
            if (rrigidbody != null)
            {
                // Destroy the rigidbody since we'll never need it again
                Destroy(rrigidbody);
                // Clear the reference so we don't get any problems later
                this.rrigidbody = null;
            }            

            // --- Change the grab type ---
            // Make a climbable one with default settings (for now)
            var climbableGrabAttach = this.gameObject.AddComponent<VRTK_ClimbableGrabAttach>();
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

            // Tell the Patback that we've welded the object
            var patback = FindObjectOfType<Patback>();
            if (patback != null)
                patback.ObjectIsWelded = true;

			if (ObjectGenerator.WeldedObjects)
				this.transform.parent = ObjectGenerator.WeldedObjects.transform;

		}

        SetColliderTrigger(false);
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
}
