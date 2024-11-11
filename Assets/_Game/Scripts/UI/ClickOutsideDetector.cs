using UnityEngine;
using UnityEngine.Events;

public class ClickOutsideDetector : MonoBehaviour
{
    [SerializeField] private LevelSelectUI levelSelectUI;
    [SerializeField] private Canvas targetCanvas;
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Camera camera = targetCanvas?.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
            if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, camera))
            {
                levelSelectUI.ReturnButton();
            }
        }
    }
}
