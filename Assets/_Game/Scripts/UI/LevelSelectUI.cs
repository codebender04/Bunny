using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LevelSelectUI : MonoBehaviour, ITransitOut
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image[] levelButtons;
    [SerializeField] private Sprite completedSprite;
    private void Start()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i + 1;
            if (LevelManager.IsLevelCompleted(levelIndex))
            {
                levelButtons[i].sprite = completedSprite;
            }
        }
    }
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
