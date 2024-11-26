using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Color hoverColor = Color.cyan;
    [SerializeField] private Color clickedColor = Color.black;
    private Color defaultColor;
    private TextMeshProUGUI buttonText;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            buttonText.color = clickedColor;
        }
    }
}
