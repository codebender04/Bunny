using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameObject pressRToRetry;
    private void Start()
    {
        GameManager.Instance.OnLevelLost += GameManager_OnLevelLost;
        GameInput.Instance.OnLevelRetried += GameInput_OnLevelRetried;
    }

    private void GameInput_OnLevelRetried(object sender, System.EventArgs e)
    {
        pressRToRetry.SetActive(false);
    }

    private void GameManager_OnLevelLost(object sender, System.EventArgs e)
    {
        pressRToRetry.SetActive(true);
    }
}
