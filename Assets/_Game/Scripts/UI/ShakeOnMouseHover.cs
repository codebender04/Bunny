using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeOnMouseHover : MonoBehaviour
{
    [SerializeField] private Vector3 rotatingAngle;
    [SerializeField] private float rotatingDuration;
    private bool isMouseHovering = false;
    private Vector3 originalRotation;
    private void Start()
    {
        originalRotation = transform.eulerAngles;
    }
    public void MouseHover()
    {
        isMouseHovering = true;
    }
    public void MouseExit()
    {
        isMouseHovering = false;
    }
    public void MouseClick()
    {
        isMouseHovering = false;
        transform.DORotate(rotatingAngle, 0f);
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
    private void Update()
    {
        if (isMouseHovering)
        {
            transform.DORotate(originalRotation - rotatingAngle, rotatingDuration);
        }
        else
        {
            transform.DORotate(originalRotation, 0.1f);
        }
    }
}
