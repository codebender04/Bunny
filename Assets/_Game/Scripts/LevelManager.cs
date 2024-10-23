using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private Level levelPrefab;
    private Character[] characterArray;
    private Level level;

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
        yield return new WaitForSeconds(2f);
        Loader.LoadNextLevel();
    }
    public void ReloadCurrentLevel()
    {
        if (level != null) DestroyCurrentLevel();

        level = Instantiate(levelPrefab);

        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i].transform.position = level.GetStartingCharacterTransformArray()[i].position;
        }
    }
    private void DestroyCurrentLevel()
    {
        Destroy(level.gameObject);
    }
}
