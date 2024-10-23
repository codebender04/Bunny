using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private static int currentlevel;
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
