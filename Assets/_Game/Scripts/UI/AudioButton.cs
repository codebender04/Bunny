using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private Sprite activeButtonSprite;
    [SerializeField] private Sprite inactiveButtonSprite;
    private Image image;
    private bool isActive = false;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public bool IsActive()
    {
        return isActive;
    }
    public void SetActive(bool active)
    {
        isActive = active;
        if (image != null)
        image.sprite = active ? activeButtonSprite : inactiveButtonSprite;
    }
}
