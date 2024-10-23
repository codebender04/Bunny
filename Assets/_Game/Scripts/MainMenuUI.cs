using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Animator titleAnimator;
    [SerializeField] private Animator mainMenuAnimator;
    [SerializeField] private Button newGameButton;
    [SerializeField] private float maxTimer;
    [SerializeField] private float minTimer;
    private float timer;
    private void Start()
    {
        timer = Random.Range(minTimer, maxTimer);
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            // Animator is not playing other animations
            if (titleAnimator.GetCurrentAnimatorStateInfo(0).length <= titleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                titleAnimator.SetTrigger(Constant.ANIM_TITLE_WAVE);
                timer = Random.Range(minTimer, maxTimer);
            }
        }
    }
    public void NewGameButton()
    {
        StartCoroutine(LoadLevelCoroutine());
        newGameButton.interactable = false;
    }
    private IEnumerator LoadLevelCoroutine()
    {
        titleAnimator.SetTrigger(Constant.ANIM_FADEOUT);
        yield return new WaitForSeconds(1.5f);
        mainMenuAnimator.SetTrigger(Constant.ANIM_MAINMENU_TRANSITOUT);
        yield return new WaitForSeconds(1.5f);
        Loader.LoadLevel(1);
    }
}
