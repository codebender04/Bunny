using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    private AudioSource audioSource;
    private float volume = 0.2f;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(Constant.PREFS_MUSIC_VOLUME, 0.2f);
        audioSource.volume = volume;
    }
    public void SetVolume(float volume)
    {
        this.volume = Mathf.Clamp01(volume);
        audioSource.volume = this.volume;
        PlayerPrefs.SetFloat(Constant.PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
