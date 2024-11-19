using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterType
{
    Green = 0,
    Purple = 1,
    Orange = 2,
    Red = 3,
}
public class GoalTile : MonoBehaviour
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private Sprite unlockedSprite;
    [SerializeField] private Sprite lockedSprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool originalState;
    private bool isUnlocked = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        TileManager.Instance.AddGoalTile(transform.position, this);
        isUnlocked = spriteRenderer.sprite == unlockedSprite;
        originalState = isUnlocked;
    }
    public CharacterType GetCharacterType()
    {
        return characterType;
    }
    public void Unlock()
    {
        if (isUnlocked) return;
        isUnlocked = true;
        animator.SetTrigger("Unlock");
    }
    public void Lock()
    {
        if (!isUnlocked) return;
        isUnlocked = false;
        animator.SetTrigger("Lock");
    }
    public bool IsUnlocked()
    {
        return isUnlocked;
    }
    public void ResetState()
    {
        if (isUnlocked && !originalState)
        {
            isUnlocked = originalState;
            animator.SetTrigger("Lock");
        }
    }
}
