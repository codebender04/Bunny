using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioClipSO audioClipSO;
    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaySound();
        }
    }
    private void PlaySound()
    {
        AudioSource.PlayClipAtPoint(audioClipSO.testClip, transform.position);
    }
}
