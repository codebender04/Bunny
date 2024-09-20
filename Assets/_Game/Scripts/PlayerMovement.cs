using UnityEngine;
using DG.Tweening;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using static GameInput;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float jumpDuration = 0.5f; 
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private GameInput gameInput;

    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private bool jumpCompleted = false;
    private bool isExecutingMovements;
    private Vector2 targetPosition;
    private void Start()
    {
        gameInput.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;
    }

    private void GameInput_OnMovementKeyPressed(object sender, OnMovementKeyPressedEventArgs e)
    {
        EnqueueMovement(e.direction);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartMovementExecution();
        }
    }
    private IEnumerator JumpToTile(Vector2 direction)
    {
        jumpCompleted = false;
        Vector2 targetPosition = (Vector2)transform.position + direction;

        if (tilemap.HasTile(tilemap.WorldToCell(targetPosition)))
        {
            transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration).OnComplete(() =>
            {
                jumpCompleted = true;
            });
        }
        else
        {
            jumpCompleted = true;
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
    private void StartMovementExecution()
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
}
