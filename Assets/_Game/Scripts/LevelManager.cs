using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private Level[] levelArray;
    [SerializeField] Character[] characterArray;
    private int currentLevelIndex = 0;
    private Level currentLevel;
    private void Awake()
    {
        Instance = this;
    }
    public void LoadLevel(int level)
    {
        if (currentLevel != null) DestroyCurrentLevel();

        currentLevelIndex = level-1;
        currentLevel = Instantiate(levelArray[currentLevelIndex]);
        CharacterMovement.OccupiedCells.Clear();

        for (int i = 0; i < characterArray.Length; i++)
        {
            characterArray[i].transform.position = currentLevel.GetStartingCharacterTransformArray()[i].position;
        }
    }
    public void ReloadCurrentLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }
    private void DestroyCurrentLevel()
    {
        Destroy(currentLevel.gameObject);
    }
}
