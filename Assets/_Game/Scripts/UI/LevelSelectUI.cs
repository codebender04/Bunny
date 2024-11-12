using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private ShakeOnMouseHover returnText;
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator animator;
    public void ReturnButton()
    {
        returnText.MouseExit();
        //TransitOut();
        gameObject.SetActive(false);
    }
    private void TransitOut()
    {
        animator.SetTrigger(Constant.ANIM_TRANSITOUT);
    }
    public void TransitIn()
    {
        animator.SetTrigger("TransitIn");
    }
}
