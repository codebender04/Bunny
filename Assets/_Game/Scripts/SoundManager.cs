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
    private void Start()
    {
        if (MovementManager.Instance == null) return;
        MovementManager.Instance.OnCharactersMoved += MovementManager_OnCharactersMoved;
    }

    private void MovementManager_OnCharactersMoved(object sender, System.EventArgs e)
    {
        PlaySound();
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
