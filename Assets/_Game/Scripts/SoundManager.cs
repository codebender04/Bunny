using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioClipSO audioClipSO;
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(Constant.PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        if (MovementManager.Instance == null) return;
        MovementManager.Instance.OnCharactersMoved += MovementManager_OnCharactersMoved;
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
    }

    private void GameInput_OnCharacterSelected(object sender, GameInput.OnCharacterSelectedEventArgs e)
    {
        PlaySound(audioClipSO.uiButtonClick, transform.position, volume);
    }

    private void MovementManager_OnCharactersMoved(object sender, System.EventArgs e)
    {
        PlaySound(audioClipSO.footstep, transform.position, volume);
    }
    public void PlayButtonClickSound()
    {
        PlaySound(audioClipSO.uiButtonClick, transform.position, volume);
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        Debug.Log(volume);
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }
    public void SetVolume(float volume)
    {
        this.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(Constant.PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
