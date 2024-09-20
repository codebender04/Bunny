using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler<OnMovementKeyPressedEventArgs> OnMovementKeyPressed;
    public class OnMovementKeyPressedEventArgs : EventArgs
    {
        public Vector2 direction;
    }
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.W.performed += ctx => OnKeyPressed(Vector2.up);
        playerInputActions.Player.A.performed += ctx => OnKeyPressed(Vector2.left);
        playerInputActions.Player.S.performed += ctx => OnKeyPressed(Vector2.down);
        playerInputActions.Player.D.performed += ctx => OnKeyPressed(Vector2.right);
    }

    private void OnKeyPressed(Vector2 direction)
    {
        OnMovementKeyPressed?.Invoke(this, new OnMovementKeyPressedEventArgs
        {
            direction = direction,
        });
    }
}
