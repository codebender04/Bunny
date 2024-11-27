using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour, ITransitOut
{
    public static SettingsUI Instance { get; private set; }
    [SerializeField] private Animator animator;
    [SerializeField] private AudioButton[] sfxButtonArray;
    [SerializeField] private AudioButton[] musicButtonArray;

    private Action activateButton;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitializeButtons();
    }
    private void OnEnable()
    {
        InitializeButtons();
    }
    private float CalculateVolume(int i, AudioButton[] buttonArray)
    {
        bool isLastActive = true;
        for (int j = 0; j < buttonArray.Length; j++)
        {
            if (j != i && buttonArray[j].IsActive())
            {
                isLastActive = false;
                break;
            }
        }
        if (isLastActive && buttonArray[i].IsActive())
        {
            buttonArray[i].SetActive(false);
            return 0f;
        }
        for (int j = 0; j <= i; j++)
        {
            buttonArray[j].SetActive(true);
        }
        for (int j = i + 1; j < buttonArray.Length; j++)
        {
            buttonArray[j].SetActive(false);
        }
        float newVolume = (i + 1) / (float)buttonArray.Length;

        return newVolume;
    }
    public void SFXButton(int i)
    {
        SoundManager.Instance.SetVolume(CalculateVolume(i, sfxButtonArray));
    }
    public void MusicButton(int i)
    {
        MusicManager.Instance.SetVolume(CalculateVolume(i, musicButtonArray));
    }
    public void Return()
    {
        StartCoroutine(nameof(TransitOut));
    }
    private IEnumerator TransitOut()
    {
        animator.SetTrigger(Constant.ANIM_TRANSITOUT);
        yield return new WaitForSeconds(.6f);
        activateButton?.Invoke();
        gameObject.SetActive(false);
    }
    public void TransitIn(Action action)
    {
        animator.SetTrigger("TransitIn");
        activateButton = action;
    }
    private void ResetButtons(AudioButton[] buttonArray)
    {
        foreach (var button in buttonArray)
        {
            button.SetActive(false);
        }
    }
    private void InitializeButtons()
    {
        int sfxIndex = Mathf.RoundToInt(PlayerPrefs.GetFloat(Constant.PREFS_SOUND_EFFECTS_VOLUME, 1f) * sfxButtonArray.Length) - 1;
        int musicIndex = Mathf.RoundToInt(PlayerPrefs.GetFloat(Constant.PREFS_MUSIC_VOLUME, 1f) * musicButtonArray.Length) - 1;

        CalculateVolume(Mathf.Max(0, sfxIndex), sfxButtonArray);
        CalculateVolume(Mathf.Max(0, musicIndex), musicButtonArray);

        if (PlayerPrefs.GetFloat(Constant.PREFS_SOUND_EFFECTS_VOLUME, 1f) == 0f) //Handle no buttons turned on situation (0f volume)
        {
            ResetButtons(sfxButtonArray);
        }
        if (PlayerPrefs.GetFloat(Constant.PREFS_MUSIC_VOLUME, 1f) == 0f) //Handle no buttons turned on situation (0f volume)
        {
            ResetButtons(musicButtonArray);
        }
        if (PlayerPrefs.GetFloat(Constant.PREFS_SOUND_EFFECTS_VOLUME, 1f) == 0.1f) //Handle first button turned on situation (.1f volume)
        {
            ResetButtons(sfxButtonArray);
            sfxButtonArray[0].SetActive(true);
        }
        if (PlayerPrefs.GetFloat(Constant.PREFS_MUSIC_VOLUME, 1f) == 0.1f) //Handle first button turned on situation (.1f volume)
        {
            ResetButtons(musicButtonArray);
            musicButtonArray[0].SetActive(true);
        }
    }
}
