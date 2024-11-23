using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioClipSO audioClipSO;
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    private float volume = 1f;

    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
    }
    private void Start()
    {
        if (MovementManager.Instance == null) return;
        MovementManager.Instance.OnCharactersMoved += MovementManager_OnCharactersMoved;
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
    }

    private void GameInput_OnCharacterSelected(object sender, GameInput.OnCharacterSelectedEventArgs e)
    {
        PlaySound();
    }

    private void MovementManager_OnCharactersMoved(object sender, System.EventArgs e)
    {
        PlaySound();
    }
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(audioClipSO.testClip, transform.position);
    }
    public void PlayButtonClickSound()
    {
        PlaySound();
    }
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }
    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
