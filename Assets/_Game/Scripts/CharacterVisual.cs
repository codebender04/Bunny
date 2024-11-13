using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private Material outlineMaterial;
    private Material originalMaterial;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        characterMovement.OnCharacterDie += CharacterMovement_OnCharacterDie;
        GameManager.Instance.OnLevelWon += GameManager_OnLevelWon;
        animator.SetTrigger(Constant.ANIM_IDLE);
        originalMaterial = spriteRenderer.material;
    }

    private void GameManager_OnLevelWon(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Constant.ANIM_DIE);
    }

    private void CharacterMovement_OnCharacterDie(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Constant.ANIM_DIE);
    }
    public void PlayIdleAnimation()
    {
        animator.SetTrigger(Constant.ANIM_IDLE);
    }
    public void PlayDieAnimation()
    {
        animator.SetTrigger(Constant.ANIM_DIE);
    }
    public void TurnOnOutline()
    {
        spriteRenderer.material = outlineMaterial;
    }
    public void TurnOffOutline()
    {
        spriteRenderer.material = originalMaterial;
    }
    public void SelectCharacter()
    {
        animator.SetTrigger(Constant.ANIM_SELECT);
        Invoke(nameof(PlayIdleAnimation), 0.2f);
    }
}
