using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using static GameInput;
using Unity.VisualScripting;
using System;
public enum MovementType
{
    Invalid = 0,
    Moveable = 1,
    MoveToDeath = 2,
    FinishMovement = 3,
}
public class CharacterMovement : MonoBehaviour
{
    public event EventHandler OnCharacterMoved;
    public event EventHandler OnCharacterFinishMovement;
    public event EventHandler OnCharacterDie;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap decorativesTilemap;
    [SerializeField] private Tilemap destructiblesTilemap;
    [SerializeField] private Tile goalTile;

    public bool ValidMovement;
    public MovementType MovementType;

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool isActivated = true;
    private Vector3Int currentCell;
    private bool isFirstStandStill = true;
    private void Start()
    {
        GameInput.Instance.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;

        currentCell = walkableTilemap.WorldToCell(transform.position);
    }

    private void GameInput_OnMovementKeyPressed(object sender, OnMovementKeyPressedEventArgs e)
    {
        if (!isActivated) return;
        movementQueue.Enqueue(e.direction);
    }
    public bool IsWalkable(Vector3Int cellPosition)
    {
        return walkableTilemap.HasTile(cellPosition) || destructiblesTilemap.HasTile(cellPosition);
    }
    public Vector3Int GetNextTargetCell(Vector2 direction)
    {
        Vector3Int targetCell = walkableTilemap.WorldToCell((Vector2)transform.position + direction);
        return targetCell;
    }

    public void ExecuteNextMove()
    {
        if (movementQueue.Count == 0) return;

        Vector2 nextMove = movementQueue.Dequeue();
        Vector3 targetPosition = transform.position + (Vector3)nextMove;
        Vector3Int targetCell = walkableTilemap.WorldToCell(targetPosition);
        if (destructiblesTilemap.HasTile(currentCell))
        {
            destructiblesTilemap.SetTile(currentCell, null);
        }
        // Perform the jump
        transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
        {
            currentCell = targetCell;
        });
    }
    public void MoveAndDie()
    {
        Vector2 nextMove = movementQueue.Dequeue();
        Vector3 targetPosition = transform.position + (Vector3)nextMove;
        Vector3Int targetCell = walkableTilemap.WorldToCell(targetPosition);

        movementQueue.Clear();
        // Perform the jump
        transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
        {
            currentCell = targetCell;
            OnCharacterDie?.Invoke(this, EventArgs.Empty);
        });
    }
    public void Shake()
    {
        // If movement is invalid, shake in place
        Vector3 originalPosition = transform.position;
        transform.DOShakePosition(shakeDuration, strength: new Vector3(shakeStrength, shakeStrength, 0), shakeVibrato, randomness: 90)
            .OnComplete(() => transform.position = originalPosition);
        if (movementQueue.Count > 0)
        {
            movementQueue.Dequeue();
        }
    }
    public void StandStill()
    {
        if (isFirstStandStill)
        {
            OnCharacterFinishMovement?.Invoke(this, EventArgs.Empty);
            Debug.Log("FinishMovement");
        }
        isFirstStandStill = false;
    }
    public bool IsAtGoal()
    {
        return decorativesTilemap.GetTile(currentCell) == goalTile;
    }
    public void Activate()
    {
        isActivated = true;
    }
    public void Deactivate()
    {
        isActivated = false;
    }
    public Queue<Vector2> GetMovementQueue()
    {
        return movementQueue;
    }
    public Vector3Int GetCurrentCell()
    {
        return currentCell;
    }
    public void ClearMovement()
    {
        movementQueue.Clear();
        isFirstStandStill = true;
        currentCell = walkableTilemap.WorldToCell(transform.position);
    }
}
