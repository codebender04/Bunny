using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using static GameInput;
using Unity.VisualScripting;
using System;

public class CharacterMovement : MonoBehaviour
{
    public event EventHandler OnCharacterMoved;
    public event EventHandler OnCharacterFinishMovement;

    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tile goalTile;
    [SerializeField] private GameInput gameInput;

    public static Dictionary<Vector3Int, CharacterMovement> OccupiedCells = new();

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool jumpCompleted = false;
    private bool isExecutingMovements;
    private bool isActivated = true;
    private Vector2 targetPosition;
    private Vector2 currentDirection;
    private Vector3Int currentCell;
    private Vector3Int targetCell;
    private void Start()
    {
        gameInput.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;

        currentCell = walkableTilemap.WorldToCell(transform.position);
        OccupiedCells.Add(currentCell, this);
    }

    private void GameInput_OnMovementKeyPressed(object sender, OnMovementKeyPressedEventArgs e)
    {
        if (!isActivated) return;
        EnqueueMovement(e.direction);
    }
    public IEnumerator JumpToTile(Vector2 direction)
    {
        OnCharacterMoved?.Invoke(this, EventArgs.Empty);

        jumpCompleted = false;
        currentCell = walkableTilemap.WorldToCell(transform.position);
        Vector2 targetPosition = (Vector2)transform.position + direction;
        targetCell = walkableTilemap.WorldToCell(targetPosition);

        // Check if the target cell is available
        if (!OccupiedCells.ContainsKey(targetCell) && IsWalkable(targetCell))
        {
            OccupiedCells[targetCell] = this;
            OccupiedCells.Remove(currentCell);

            transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
            {
                jumpCompleted = true;
            });
        }
        else if (OccupiedCells.ContainsKey(targetCell) && OccupiedCells[targetCell].HasValidMoves())
        {
            OccupiedCells[targetCell] = this;
            OccupiedCells.Remove(currentCell);

            transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
            {
                jumpCompleted = true;
            });
        }
        else
        {
            // If the target cell is occupied, shake the character
            Vector3 originalPosition = transform.position;
            transform.DOShakePosition(shakeDuration, strength: new Vector3(shakeStrength, shakeStrength, 0), shakeVibrato, randomness: 90, snapping: false, fadeOut: true, ShakeRandomnessMode.Harmonic)
                .OnComplete(() =>
                {
                    transform.position = originalPosition;
                    jumpCompleted = true;
                });
        }

        // Wait until the jump is completed
        while (!jumpCompleted)
        {
            yield return null;
        }
    }
    private bool HasValidMoves()
    {
        if (movementQueue.Count == 0) return false;

        // Check if the next move is to an unoccupied, valid tile
        Vector2 nextMove = movementQueue.Peek();
        Vector3Int potentialTargetCell = walkableTilemap.WorldToCell((Vector2)transform.position + nextMove);
        if (!OccupiedCells.ContainsKey(potentialTargetCell) && IsWalkable(potentialTargetCell))
        {
            return true;
        }
        else if (OccupiedCells.ContainsKey(potentialTargetCell) && OccupiedCells[potentialTargetCell].HasValidMoves())
        {
            return true;
        }
        return false;
    }
    private bool IsWalkable(Vector3Int cellPosition)
    {
        return walkableTilemap.HasTile(cellPosition);
    }
    private void EnqueueMovement(Vector2 direction)
    {
        movementQueue.Enqueue(direction);
    }
    private IEnumerator ExecuteMovements()
    {
        isExecutingMovements = true;

        while (movementQueue.Count > 0)
        {
            currentDirection = movementQueue.Dequeue();
            yield return JumpToTile(currentDirection);
        }

        isExecutingMovements = false;

        OnCharacterFinishMovement?.Invoke(this, EventArgs.Empty);
    }
    public void StartMovementExecution()
    {
        if (!isExecutingMovements)
        {
            StartCoroutine(nameof(ExecuteMovements));
        }
    }
    public bool IsAtGoal()
    {
        return walkableTilemap.GetTile(targetCell) == goalTile;
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
}
