using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using static GameInput;
using Unity.VisualScripting;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f; 
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeStrength = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameInput gameInput;

    private static Dictionary<Vector3Int, CharacterMovement> occupiedCells = new();

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool jumpCompleted = false;
    private bool isExecutingMovements;
    private bool isActivated = true;
    private Vector2 targetPosition;
    private Vector3Int currentCell;
    private Vector3Int targetCell;
    private void Start()
    {
        gameInput.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;

        currentCell = tilemap.WorldToCell(transform.position);
        occupiedCells.Add(currentCell, this);
    }

    private void GameInput_OnMovementKeyPressed(object sender, OnMovementKeyPressedEventArgs e)
    {
        if (!isActivated) return;
        EnqueueMovement(e.direction);
    }
    public IEnumerator JumpToTile(Vector2 direction)
    {
        jumpCompleted = false;
        currentCell = tilemap.WorldToCell(transform.position);
        Vector2 targetPosition = (Vector2)transform.position + direction;
        targetCell = tilemap.WorldToCell(targetPosition);

        // Check if the target cell is available
        if (!occupiedCells.ContainsKey(targetCell) && tilemap.HasTile(targetCell))
        {
            // Reserve the target cell
            occupiedCells[targetCell] = this;

            // Remove the current cell from the occupiedCells map after jumping
            occupiedCells.Remove(currentCell);

            // Perform the jump movement
            transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
            {
                jumpCompleted = true;
            });
        }
        else if (occupiedCells.ContainsKey(targetCell) && occupiedCells[targetCell].HasValidMoves())
        {
            occupiedCells[targetCell] = this;
            occupiedCells.Remove(currentCell);
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
        Vector3Int potentialTargetCell = tilemap.WorldToCell((Vector2)transform.position + nextMove);
        if (!occupiedCells.ContainsKey(potentialTargetCell) && tilemap.HasTile(potentialTargetCell))
        {
            return true;
        }
        return false;
    }
    private void EnqueueMovement(Vector2 direction)
    {
        movementQueue.Enqueue(direction);
    }
    public void StartMovementExecution()
    {
        if (!isExecutingMovements)
        {
            StartCoroutine(nameof(ExecuteMovements));
        }
    }
    private IEnumerator ExecuteMovements()
    {
        isExecutingMovements = true;

        while (movementQueue.Count > 0)
        {
            Vector2 direction = movementQueue.Dequeue();
            yield return JumpToTile(direction);
        }

        isExecutingMovements = false;
    }
    public void Activate()
    {
        isActivated = true;
    }
    public void Deactivate()
    {
        isActivated = false;
    }
}
