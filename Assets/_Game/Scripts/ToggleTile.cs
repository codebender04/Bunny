using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "ToggleTile", menuName = "Tiles/ToggleTile")]
public class ToggleTile : Tile
{
    private bool isActive = false;
    [SerializeField] private Tile activatedSprite;
    [SerializeField] private Tile deactivatedSprite;
    [SerializeField] private GameObject tileAnimationPrefab;
    [SerializeField] private Tilemap tilemap;
    public void ToggleState(Vector3Int position, Tilemap tilemap)
    {
        isActive = !isActive;
        Vector3 worldPosition = tilemap.CellToWorld(position) + tilemap.tileAnchor;

        // Instantiate or retrieve the animation GameObject
        GameObject animationInstance = Instantiate(tileAnimationPrefab, worldPosition, Quaternion.identity);
        Animator animator = animationInstance.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(isActive ? "Activate" : "Deactivate");
        }
        tilemap.SetTile(position, activatedSprite);
        // Destroy the animation instance after animation completes
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(animationInstance, animationLength);
    }
}
