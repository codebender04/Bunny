using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static MovementManager Instance;
    [SerializeField] private CharacterMovement[] characterMovementArray;
    public bool allCharactersReadyToMove = false;
    private int charactersToMove = 0;
    private int charactersFinishedMovement = 0;
    // Dictionary to hold each character's announced target cell
    public static Dictionary<CharacterMovement, Vector3Int> PotentialCellDict = new Dictionary<CharacterMovement, Vector3Int>();

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (var character in characterMovementArray)
        {
            character.OnCharacterFinishMovement += Character_OnCharacterFinishMovement;
        }
    }

    private void Character_OnCharacterFinishMovement(object sender, System.EventArgs e)
    {
        charactersFinishedMovement++;
    }

    public void AnnounceMovement(CharacterMovement characterMovement, Vector3Int targetCell)
    {
        charactersToMove++;
        PotentialCellDict[characterMovement] = targetCell;

        // Wait for all characters to announce their movements
        if (charactersToMove >= characterMovementArray.Length - charactersFinishedMovement)
        {
            // Identify characters that want to move to the same cell
            var duplicateCells = PotentialCellDict.GroupBy(x => x.Value)
                                                  .Where(group => group.Count() > 1)
                                                  .Select(group => group.Key)
                                                  .ToList();

            // Invalidate movements for all characters targeting the same cell
            foreach (var cell in duplicateCells)
            {
                var conflictedCharacters = PotentialCellDict.Where(x => x.Value == cell)
                                                            .Select(x => x.Key);
                foreach (var character in conflictedCharacters)
                {
                    character.ValidMovement = false; // Invalidate all conflicting movements
                }
            }

            // Allow valid moves for non-conflicting characters
            foreach (var kvp in PotentialCellDict)
            {
                if (!duplicateCells.Contains(kvp.Value))
                {
                    kvp.Key.ValidMovement = true;
                    CharacterMovement.OccupiedCells[kvp.Value] = kvp.Key;
                }
            }

            // Reset for the next round
            charactersToMove = 0;
            charactersFinishedMovement = 0;
            allCharactersReadyToMove = true;
        }
    }

    public bool AreAllCharactersReadyToMove()
    {
        return allCharactersReadyToMove;
    }
}
