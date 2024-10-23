using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public static Loader Instance;
    private static int currentlevel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static void LoadLevel(int level)
    {
        currentlevel = level;
        string levelName = "Level " + level.ToString();

        SceneManager.LoadScene(levelName);
    }
    public static void LoadNextLevel()
    {
        currentlevel++;
        LoadLevel(currentlevel);
    }
}
