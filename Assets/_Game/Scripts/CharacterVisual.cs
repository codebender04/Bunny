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
        animator.SetTrigger(Constant.ANIM_IDLE);
    }

    private void CharacterMovement_OnCharacterDie(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Constant.ANIM_DIE);
    }
    public void PlayIdleAnimation()
    {
        animator.SetTrigger(Constant.ANIM_IDLE);
    }
}
