using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private ShakeOnMouseHover returnText;
    [SerializeField] private GameObject panel;
    public void ReturnButton()
    {
        returnText.MouseExit();
        gameObject.SetActive(false);
    }
}
