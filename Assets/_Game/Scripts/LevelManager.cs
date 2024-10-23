using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

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
        StartCoroutine(nameof(LoadNextLevelCoroutine));
    }
    private IEnumerator LoadNextLevelCoroutine()
    {
        level.FadeOut();
        yield return new WaitForSeconds(2f);
        Loader.LoadNextLevel();
    }
    public void ReloadCurrentLevel()
    {
        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i].transform.position = level.GetStartingCharacterTransformArray()[i].position;
        }
    }
}
