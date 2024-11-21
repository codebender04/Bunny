using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Button backToMainMenuButton;
    [SerializeField] private Button skipLevelButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        backToMainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
