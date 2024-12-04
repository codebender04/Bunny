using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsUI : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(BackToMainMenu), 20f);
    }
    private void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
