using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;
    public event EventHandler<OnMovementKeyPressedEventArgs> OnMovementKeyPressed;
    public class OnMovementKeyPressedEventArgs : EventArgs
    {
        public Vector2 direction;
    }
    public event EventHandler<OnCharacterSelectedEventArgs> OnCharacterSelected;
    public class OnCharacterSelectedEventArgs : EventArgs
    {
        public Character selectedCharacter;
    }
    public event EventHandler OnLevelRetried;
    public event EventHandler OnStartMovementSequence;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.MoveUp.performed += ctx => OnKeyPressed(Vector2.up);
        playerInputActions.Player.MoveLeft.performed += ctx => OnKeyPressed(Vector2.left);
        playerInputActions.Player.MoveDown.performed += ctx => OnKeyPressed(Vector2.down);
        playerInputActions.Player.MoveRight.performed += ctx => OnKeyPressed(Vector2.right);
        playerInputActions.Player.LeftClick.performed += LeftClick_performed;
        playerInputActions.Player.StartMovementSequence.performed += StartMovementSequence_performed;
        playerInputActions.Player.RetryLevel.performed += RetryLevel_performed;
    }
    private void OnDestroy()
    {
        playerInputActions.Player.MoveUp.performed -= ctx => OnKeyPressed(Vector2.up);
        playerInputActions.Player.MoveLeft.performed -= ctx => OnKeyPressed(Vector2.left);
        playerInputActions.Player.MoveDown.performed -= ctx => OnKeyPressed(Vector2.down);
        playerInputActions.Player.MoveRight.performed -= ctx => OnKeyPressed(Vector2.right);
        playerInputActions.Player.LeftClick.performed -= LeftClick_performed;
        playerInputActions.Player.StartMovementSequence.performed -= StartMovementSequence_performed;
        playerInputActions.Player.RetryLevel.performed -= RetryLevel_performed;

        playerInputActions.Dispose();
    }
    private void RetryLevel_performed(InputAction.CallbackContext obj)
    {
        OnLevelRetried?.Invoke(this, EventArgs.Empty);
    }

    private void StartMovementSequence_performed(InputAction.CallbackContext obj)
    {
        OnStartMovementSequence?.Invoke(this, EventArgs.Empty);
    }

    private void LeftClick_performed(InputAction.CallbackContext obj)
    {
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

        if (rayHit && rayHit.collider.TryGetComponent(out Character hitCharacter))
        {
            OnCharacterSelected?.Invoke(this, new OnCharacterSelectedEventArgs
            {
                selectedCharacter = hitCharacter,
            });
        }
    }

    private void OnKeyPressed(Vector2 direction)
    {
        OnMovementKeyPressed?.Invoke(this, new OnMovementKeyPressedEventArgs
        {
            direction = direction,
        });
    }
    
}
