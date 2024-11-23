using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour, ITransitOut
{
    [SerializeField] private Animator animator;

    public void Return()
    {
        StartCoroutine(nameof(TransitOut));
    }
    private IEnumerator TransitOut()
    {
        animator.SetTrigger(Constant.ANIM_TRANSITOUT);
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }
    public void TransitIn()
    {
        animator.SetTrigger("TransitIn");
    }
}
