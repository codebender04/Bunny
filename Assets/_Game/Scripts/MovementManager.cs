using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;
    public event EventHandler OnCharactersMoved;

    private CharacterMovement[] characterMovementArray;
    private Dictionary<CharacterMovement, Vector3Int> nextCellDict = new Dictionary<CharacterMovement, Vector3Int>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        characterMovementArray = new CharacterMovement[GameManager.Instance.GetCharacterArray().Length];
        for (int i = 0; i < characterMovementArray.Length; i++)
        {
            characterMovementArray[i] = GameManager.Instance.GetCharacterArray()[i].GetCharacterMovement();
        }
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
                    nextCellDict[character] = targetCell;
                }
                else
                {
                    // Character has no more moves
                    character.MovementType = MovementType.FinishMovement;
                    nextCellDict[character] = character.GetCurrentCell();
                }
            }
            HandleSameCellMovement();
            HandleDifferentCellMovement();

            ExecuteMovement();

            yield return new WaitForSeconds(0.2f);

            RemoveFinishedCharacters();
        }
    }
    private void HandleSameCellMovement()
    {
        var duplicates = nextCellDict
            .GroupBy(pair => pair.Value)            // Group by the Vector3Int value
            .Where(group => group.Count() > 1)      // Filter groups where there's more than one CharacterMovement
            .Select(group => group.Select(pair => pair.Key));  // Select the CharacterMovements in each group
        foreach (var group in duplicates)
        {
            foreach (CharacterMovement characterMovement in group)
            {
                characterMovement.MovementType = MovementType.Invalid;
            }
        }
    }
    private void HandleDifferentCellMovement()
    {
        var nonDuplicates = nextCellDict
                .GroupBy(pair => pair.Value)            // Group by the Vector3Int value
                .Where(group => group.Count() == 1)     // Only keep groups where there's exactly one CharacterMovement
                .Select(group => group.First().Key);    // Get the CharacterMovement for the unique Vector3Int

        foreach (CharacterMovement characterMovement in nonDuplicates)
        {
            if (characterMovement.GetMovementQueue().Count > 0)
            {
                Vector3Int currentCell = characterMovement.GetCurrentCell();
                Vector3Int targetCell = nextCellDict[characterMovement];

                foreach (var otherCharacter in nonDuplicates)
                {
                    if (otherCharacter == characterMovement) continue;

                    Vector3Int otherCurrentCell = otherCharacter.GetCurrentCell();
                    Vector3Int otherTargetCell = nextCellDict[otherCharacter];

                    // Detect cross-passing movement
                    if (currentCell == otherTargetCell && targetCell == otherCurrentCell)
                    {
                        characterMovement.MovementType = MovementType.Invalid;
                        otherCharacter.MovementType = MovementType.Invalid;
                    }
                }
                if (characterMovement.MovementType != MovementType.Invalid) 
                { 
                    if (!characterMovement.IsWalkable(nextCellDict[characterMovement]))
                    {
                        characterMovement.MovementType = MovementType.MoveToDeath;
                    }
                    else
                    {
                        characterMovement.MovementType = MovementType.Moveable;
                    }
                }
            }
            else
            {
                characterMovement.MovementType = MovementType.FinishMovement;
                nextCellDict[characterMovement] = characterMovement.GetCurrentCell();
            }
        }
    }
    private void ExecuteMovement()
    {
        // Now, execute movements for all characters that have valid moves
        foreach (CharacterMovement character in characterMovementArray)
        {
            switch (character.MovementType)
            {
                case MovementType.Invalid:
                    character.Shake();
                    break;
                case MovementType.Moveable:
                    OnCharactersMoved?.Invoke(this, EventArgs.Empty);
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
    }
    private void RemoveFinishedCharacters()
    {
        // Clear the dictionary only for cells where characters moved, not for characters that finished early
        List<CharacterMovement> charactersToRemove = new List<CharacterMovement>();

        // Loop through the dictionary and find the CharacterMovements to remove
        foreach (var entry in nextCellDict)
        {
            // Check the MovementType and add the CharacterMovement key to the list if it matches the condition
            if (entry.Key.MovementType == MovementType.Moveable || entry.Key.MovementType == MovementType.MoveToDeath)
            {
                charactersToRemove.Add(entry.Key);  // Add the CharacterMovement (not Vector3Int) to remove
            }
        }

        // Remove the CharacterMovement entries from the dictionary
        foreach (var character in charactersToRemove)
        {
            nextCellDict.Remove(character);
        }
    }

    public void ResetCellDict()
    {
        nextCellDict.Clear();
    }
}
