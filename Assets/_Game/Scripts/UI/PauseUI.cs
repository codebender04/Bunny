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
        skipLevelButton.onClick.AddListener(() =>
        {
            Loader.LoadNextLevel();
        });
    }
    private void Start()
    {
        GameInput.Instance.OnLevelPaused += GameInput_OnLevelPaused;
        gameObject.SetActive(false);
    }

    private void GameInput_OnLevelPaused(object sender, System.EventArgs e)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
