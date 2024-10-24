using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterClone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float opacity;
    private Character character;
    private bool jumpCompleted = false;
    public static Dictionary<Vector3Int, CharacterMovement> OccupiedCells = new();

    public void OnInit(Character character)
    {
        this.character = character;
        spriteRenderer.sprite = character.GetSpriteRenderer().sprite;
        spriteRenderer.transform.localPosition = character.GetVisualOffset();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
    }
    public IEnumerator JumpToDirection(Vector2 direction)
    {
        jumpCompleted = false;
        Vector2 targetPosition = character.GetLastClonePosition() + direction;
        transform.DOJump(targetPosition, 0.2f, 1, 0.2f)
            .OnComplete(() =>
            {
                jumpCompleted = true;
            });
        while (!jumpCompleted)
        {
            yield return null;
        }
    }
    public bool IsJumping() 
    { 
        return !jumpCompleted;
    }
}
