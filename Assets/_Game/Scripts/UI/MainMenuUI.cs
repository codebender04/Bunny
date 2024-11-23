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
    [SerializeField] private Button levelsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private LevelSelectUI levelSelectUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private float maxTimer;
    [SerializeField] private float minTimer;
    private float timer;
    private void Awake()
    {
        newGameButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadLevelCoroutine());
            newGameButton.interactable = false;
            SoundManager.Instance.PlayButtonClickSound();
        });
        levelsButton.onClick.AddListener(() =>
        {
            levelSelectUI.gameObject.SetActive(true);
            levelSelectUI.TransitIn();
            SoundManager.Instance.PlayButtonClickSound();
        });
        settingsButton.onClick.AddListener(() =>
        {
            settingsUI.gameObject.SetActive(true);
            settingsUI.TransitIn();
            SoundManager.Instance.PlayButtonClickSound();
        });
    }
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
    private IEnumerator LoadLevelCoroutine()
    {
        titleAnimator.SetTrigger(Constant.ANIM_FADEOUT);
        yield return new WaitForSeconds(1.5f);
        mainMenuAnimator.SetTrigger(Constant.ANIM_TRANSITOUT);
        yield return new WaitForSeconds(1.5f);
        Loader.LoadLevel(1);
    }
}
