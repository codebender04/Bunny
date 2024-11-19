using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Sprite unlocked;
    [SerializeField] private Sprite locked;
    [SerializeField] private int level;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            Loader.LoadLevel(level);
        });
        GetComponent<Image>().sprite = PlayerPrefs.GetInt(Constant.PREFS_FINISHLEVELINDEX, 0) < level ? locked : unlocked;
    }
}
