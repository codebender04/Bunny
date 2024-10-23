using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterMovement characterMovement;
    private bool isDead = false;
    private void Start()
    {
        characterMovement.OnCharacterDie += CharacterMovement_OnCharacterDie;
        GameManager.Instance.OnLevelWon += GameManager_OnLevelWon;
        GameInput.Instance.OnLevelRetried += GameInput_OnLevelRetried;
        animator.SetTrigger(Constant.ANIM_IDLE);
    }

    private void GameInput_OnLevelRetried(object sender, System.EventArgs e)
    {
        //Dead character does not play reset animation
        if (!isDead)
        {
            animator.SetTrigger(Constant.ANIM_DIE);
        }
        isDead = false;
    }

    private void GameManager_OnLevelWon(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Constant.ANIM_DIE);
    }

    private void CharacterMovement_OnCharacterDie(object sender, System.EventArgs e)
    {
        isDead = true;
        animator.SetTrigger(Constant.ANIM_DIE);
    }
    public void PlayIdleAnimation()
    {
        animator.SetTrigger(Constant.ANIM_IDLE);
    }
}
