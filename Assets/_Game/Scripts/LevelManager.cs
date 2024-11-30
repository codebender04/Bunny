using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public event EventHandler OnLoadNextLevel;

    [SerializeField] private Level level;

    private Character[] characterArray;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        characterArray = GameManager.Instance.GetCharacterArray();
        GameManager.Instance.OnLevelWon += GameManager_OnLevelWon;
    }
    private void GameManager_OnLevelWon(object sender, System.EventArgs e)
    {
        LoadNextLevel();
    }
    public void LoadNextLevel()
    {
        StartCoroutine(nameof(LoadNextLevelCoroutine));
    }
    private IEnumerator LoadNextLevelCoroutine()
    {
        level.FadeOut();
        OnLoadNextLevel?.Invoke(this, EventArgs.Empty);
        yield return new WaitForSeconds(2f);
        Loader.LoadNextLevel();
    }
    public void ResetCharactersPosition()
    {
        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i].transform.position = level.GetStartingCharacterPositionArray()[i];
        }
    }
    public static void MarkLevelAsCompleted(int levelIndex)
    {
        PlayerPrefs.SetInt($"Level_{levelIndex}_Completed", 1);
        PlayerPrefs.Save();
    }
    public static bool IsLevelCompleted(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level_{levelIndex}_Completed", 0) == 1;
    }
}
