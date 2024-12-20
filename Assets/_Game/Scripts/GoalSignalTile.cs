using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSignalTile : MonoBehaviour
{
    private Animator animator;
    private bool isActivated = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        TileManager.Instance.AddGoalSignalTile(transform.position, this);
        LevelManager.Instance.OnLoadNextLevel += LevelManager_OnLoadNextLevel;
    }

    private void LevelManager_OnLoadNextLevel(object sender, System.EventArgs e)
    {
        animator.SetTrigger(Constant.ANIM_FADEOUT);
    }

    public void ToggleState()
    {
        isActivated = !isActivated;
        if (isActivated)
        {
            animator.SetTrigger("Activate");
        }
        else
        {
            animator.SetTrigger("Deactivate");
        }
    }
    public void ResetState()
    {
        if (isActivated)
        {
            animator.SetTrigger("Deactivate");
            isActivated = false;
        }
    }
    public bool IsActivated()
    {
        return isActivated;
    }
}
