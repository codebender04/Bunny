using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Transform[] characterTransformArray;
    
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public Transform[] GetStartingCharacterTransformArray()
    {
        return characterTransformArray;
    }
    public void FadeOut()
    {
        animator.SetTrigger(Constant.ANIM_FADEOUT);
    }
}
