using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void TransitIn()
    {
        animator.SetTrigger("TransitIn");
    }
}
