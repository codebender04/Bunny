using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Animator titleAnimator;
    [SerializeField] private float maxTimer;
    private float timer;
    private void Start()
    {
        timer = maxTimer;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            titleAnimator.SetTrigger(Constant.ANIM_TITLE_WAVE);
            timer = maxTimer;
        }
    }
    public void NewGameButton()
    {
        Loader.LoadLevel(1);
    }
}
