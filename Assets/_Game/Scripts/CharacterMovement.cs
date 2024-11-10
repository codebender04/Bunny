using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using static GameInput;
using Unity.VisualScripting;
using System;
using UnityEngine.TextCore.Text;
public enum MovementType
{
    Invalid = 0,
    Moveable = 1,
    MoveToDeath = 2,
    FinishMovement = 3,
}
public class CharacterMovement : MonoBehaviour
{
    public event EventHandler OnCharacterFinishMovement;
    public event EventHandler OnCharacterDie;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap destructiblesTilemap;

    public bool ValidMovement;
    public MovementType MovementType;

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool isActivated = true;
    private Vector3Int currentCell;
    private bool isFirstStandStill = true;
    private Vector3Int targetCell;
    private void Start()
    {
        GameInput.Instance.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;

        currentCell = walkableTilemap.WorldToCell(transform.position);
        targetCell = walkableTilemap.WorldToCell(transform.position);
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
        targetCell = walkableTilemap.WorldToCell(targetPosition);

        Level.Instance.CheckBrokenTile(this);

        transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
        {
            currentCell = targetCell;
            Level.Instance.CheckSignalTile(this);
        });
    }
    public void MoveAndDie()
    {
        Vector2 nextMove = movementQueue.Dequeue();
        Vector3 targetPosition = transform.position + (Vector3)nextMove;
        targetCell = walkableTilemap.WorldToCell(targetPosition);

        Level.Instance.CheckBrokenTile(this);

        movementQueue.Clear();

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
    public Vector3Int GetTargetCell()
    {
        return targetCell;
    }
    public void ClearMovement()
    {
        movementQueue.Clear();
        isFirstStandStill = true;
        currentCell = walkableTilemap.WorldToCell(transform.position);
    }
}
