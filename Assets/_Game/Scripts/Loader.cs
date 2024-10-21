using UnityEngine.SceneManagement;

public static class Loader
{
    private static int currentlevel;
    public static void LoadLevel(int level)
    {
        string levelName = "Level " + level.ToString();

        SceneManager.LoadScene(levelName);
    }
}
