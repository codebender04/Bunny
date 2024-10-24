using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;
    private void Start()
    {
        characterMovement.OnCharacterDie += CharacterMovement_OnCharacterDie;
        GameManager.Instance.OnLevelWon += GameManager_OnLevelWon;
        animator.SetTrigger(Constant.ANIM_IDLE);
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
}
