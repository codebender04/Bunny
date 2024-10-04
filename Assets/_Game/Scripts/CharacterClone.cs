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

    public void OnInit(Character character)
    {
        this.character = character;
        spriteRenderer.sprite = character.GetSpriteRenderer().sprite;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, opacity);
    }
    public void JumpToTile(Vector2 direction)
    {
        jumpCompleted = false;
        Vector2 targetPosition = character.GetLastClonePosition() + direction;
        transform.DOJump(targetPosition, 0.2f, 1, 0.2f)
            .OnComplete(() =>
            {
                jumpCompleted = true;
            });
    }
    public bool IsJumping() 
    { 
        return !jumpCompleted;
    }
}
