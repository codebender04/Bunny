using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverOutline : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial; // Assign the outline material here
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void OnMouseEnter()
    {
        spriteRenderer.material = outlineMaterial;
    }

    private void OnMouseExit()
    {
        spriteRenderer.material = originalMaterial; // Switch back to original material
    }
}
