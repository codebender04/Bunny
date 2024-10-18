using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;
    [SerializeField] private CharacterMovement[] characterMovementArray;
    private Dictionary<Vector3Int, CharacterMovement> nextCellDict = new Dictionary<Vector3Int, CharacterMovement>();
    private Dictionary<CharacterMovement, Vector3Int> newNextCellDict = new Dictionary<CharacterMovement, Vector3Int>();

    private void Awake()
    {
        Instance = this;
    }

    public void StartSynchronizedMovement()
    {
        StartCoroutine(SynchronizedMovementRoutine());
    }

    private IEnumerator SynchronizedMovementRoutine()
    {
        int maxSteps = 0;
        // Find the longest queue length
        foreach (CharacterMovement character in characterMovementArray)
        {
            maxSteps = Mathf.Max(maxSteps, character.GetMovementQueue().Count);
        }
        // Execute each step in the queue for all characters
        for (int step = 0; step <= maxSteps; step++)
        {
            // First, announce the next position for all characters
            foreach (CharacterMovement character in characterMovementArray)
            {
                if (character.GetMovementQueue().Count > 0)
                {
                    Vector2 direction = character.GetMovementQueue().Peek();
                    Vector3Int targetCell = character.GetNextTargetCell(direction);
                    // Check if the target cell is already occupied by another character moving
                    newNextCellDict[character] = targetCell;
                    if (!character.IsWalkable(targetCell))
                    {
                        character.MovementType = MovementType.MoveToDeath;
                    }
                    else if (!nextCellDict.ContainsKey(targetCell))
                    {
                        nextCellDict[targetCell] = character;
                        character.MovementType = MovementType.Moveable;
                    }
                    else
                    {
                        // Conflict detected: Both characters try to move to the same cell, cancel both
                        character.MovementType = MovementType.Invalid;
                        nextCellDict[targetCell].MovementType = MovementType.Invalid;
                    }
                }
                else
                {
                    // Character has no more moves
                    character.MovementType = MovementType.FinishMovement;
                    newNextCellDict[character] = character.GetCurrentCell();
                    if (nextCellDict.ContainsKey(character.GetCurrentCell()))
                    {
                        nextCellDict[character.GetCurrentCell()].MovementType = MovementType.Invalid;
                    }
                }
            }
            var duplicates = newNextCellDict
                .GroupBy(pair => pair.Value)            // Group by the Vector3Int value
                .Where(group => group.Count() > 1)      // Filter groups where there's more than one CharacterMovement
                .Select(group => group.Select(pair => pair.Key));  // Select the CharacterMovements in each group
            foreach (var group in duplicates)
            {
                Debug.Log("Duplicate Vector3Int shared by:");
                foreach (CharacterMovement characterMovement in group)
                {
                    characterMovement.MovementType = MovementType.Invalid;
                }
            }
            // Now, execute movements for all characters that have valid moves
            foreach (CharacterMovement character in characterMovementArray)
            {
                switch (character.MovementType)
                {
                    case MovementType.Invalid:
                        character.Shake();
                        break;
                    case MovementType.Moveable:
                        character.ExecuteNextMove();
                        break;
                    case MovementType.FinishMovement:
                        character.StandStill();
                        break;
                    case MovementType.MoveToDeath:
                        character.MoveAndDie();
                        break;
                }
            }

            // Wait for all movements to complete before the next step
            yield return new WaitForSeconds(0.2f);
            // Clear the dictionary only for cells where characters moved, not for characters that finished early
            List<Vector3Int> cellsToRemove = new List<Vector3Int>();
            foreach (var entry in nextCellDict)
            {
                if (entry.Value.MovementType == MovementType.Moveable || entry.Value.MovementType == MovementType.MoveToDeath)
                {
                    cellsToRemove.Add(entry.Key); // Only remove cells where the character actually moved
                }
            }

            // Remove only the cells where movement occurred
            foreach (var cell in cellsToRemove)
            {
                nextCellDict.Remove(cell);
            }
        }
    }
    public void ResetCellDict()
    {
        nextCellDict.Clear();
    }
}
