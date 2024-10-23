using UnityEngine.SceneManagement;

public static class Loader
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
        SceneManager.LoadScene(currentlevel);
    }
}
