using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] private ShakeOnMouseHover returnText;
    public void ReturnButton()
    {
        returnText.MouseExit();
        gameObject.SetActive(false);
    }
}
