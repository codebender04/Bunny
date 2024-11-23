using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color hoverColor = Color.cyan;
    private Color defaultColor;
    private TextMeshProUGUI buttonText;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
    }
    private void OnDisable()
    {
        if (buttonText != null)
            buttonText.color = defaultColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
            buttonText.color = defaultColor;
    }
}
