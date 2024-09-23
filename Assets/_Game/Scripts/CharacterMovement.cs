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
    [SerializeField] private float shakeStrengh = 0.1f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameInput gameInput;

    private static HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool jumpCompleted = false;
    private bool isExecutingMovements;
    private bool isActivated = true;
    private Vector2 targetPosition;
    private Vector3Int currentCell;
    private void Start()
    {
        gameInput.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;

        currentCell = tilemap.WorldToCell(transform.position);
        occupiedCells.Add(currentCell);
    }

    private void GameInput_OnMovementKeyPressed(object sender, OnMovementKeyPressedEventArgs e)
    {
        if (!isActivated) return;
        EnqueueMovement(e.direction);
    }
    private IEnumerator JumpToTile(Vector2 direction)
    {
        jumpCompleted = false;
        currentCell = tilemap.WorldToCell(transform.position);
        Vector2 targetPosition = (Vector2)transform.position + direction;
        Vector3Int targetCell = tilemap.WorldToCell(targetPosition);

        if (!occupiedCells.Contains(targetCell) && tilemap.HasTile(targetCell))
        {
            occupiedCells.Add(targetCell);
            occupiedCells.Remove(currentCell);

            transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
            {
                jumpCompleted = true;
            });
        }
        else
        {
            Vector3 originalPosition = transform.position;
            transform.DOShakePosition(shakeDuration, strength: new Vector3(shakeStrengh, shakeStrengh, 0), shakeVibrato, randomness: 90, snapping: false, fadeOut: true, ShakeRandomnessMode.Harmonic)
                .OnComplete(() =>
                {
                    transform.position = originalPosition;
                    jumpCompleted = true;
                });
        }
        while (!jumpCompleted)
        {
            yield return null;
        }
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
