using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Animator titleAnimator;
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
            titleAnimator.SetTrigger(Constant.ANIM_TITLE_WAVE);
            timer = Random.Range(minTimer, maxTimer);
        }
    }
    public void NewGameButton()
    {
        Loader.LoadLevel(1);
    }
}
