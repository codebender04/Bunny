using UnityEngine;
using UnityEngine.Events;

public class ClickOutsideDetector : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Canvas targetCanvas;

    private ITransitOut transitOut;
    private void Awake()
    {
        transitOut = panel.GetComponent<ITransitOut>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Camera camera = targetCanvas?.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
            if (!RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), Input.mousePosition, camera))
            {
                transitOut.Return();
            }
        }
    }
}
