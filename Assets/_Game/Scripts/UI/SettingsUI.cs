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
            return 0;
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
}
