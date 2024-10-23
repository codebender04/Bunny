using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private Level levelPrefab;
    private Character[] characterArray;
    private Level level;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        characterArray = GameManager.Instance.GetCharacterArray();
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
