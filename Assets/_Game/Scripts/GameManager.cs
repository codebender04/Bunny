using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using static UnityEditor.FilePathAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public event EventHandler OnLevelLost;
    public event EventHandler OnLevelWon;

    [SerializeField] private Character[] characterArray;
    [SerializeField] private Tilemap destructiblesTilemap;
    [SerializeField] private Tile destructiblesTile;

    private List<Vector3Int> destructiblesPositionList;
    private int charactersFinishedMovement = 0;
    private Character selectedCharacter;
    private bool levelHasEnded = false;
    private void Awake()
    {
        Instance = this;
        destructiblesPositionList = new List<Vector3Int>();
    }
    private void Start()
    {
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
        GameInput.Instance.OnStartMovementSequence += GameInput_OnStartMovementSequence;
        GameInput.Instance.OnLevelRetried += GameInput_OnLevelRetried;

        foreach (Character character in characterArray)
        {
            character.GetCharacterMovement().OnCharacterFinishMovement += CharacterMovement_OnCharacterFinishMovement;
        }
        // Select character 1 by default
        selectedCharacter = characterArray[0];
        foreach (Character character in characterArray)
        {
            character.ToggleMovement(character == selectedCharacter);
        }
        BoundsInt bounds = destructiblesTilemap.cellBounds;
        TileBase[] destructibles = destructiblesTilemap.GetTilesBlock(bounds);
        for (int x = bounds.xMin; x < bounds.xMax;  x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);
                if (destructiblesTilemap.HasTile(localLocation))
                {
                    destructiblesPositionList.Add(localLocation);
                }
            }
        }
    }

    private void GameInput_OnLevelRetried(object sender, EventArgs e)
    {
        levelHasEnded = false;
        characterArray[0].GetCharacterMovement().Activate();
        MovementManager.Instance.ResetCellDict();
        for (int i = 0; i < characterArray.Length; i++)
        {
            if (characterArray[i].IsDead()) continue;
            Destroy(Instantiate(characterArray[i].GetDeadDummy(), characterArray[i].GetCharacterVisual().transform.position, Quaternion.identity), 1f);
        }
        LevelManager.Instance.ResetCharactersPosition();
        foreach (Character character in characterArray)
        {
            character.ResetCharacter();
        }
        foreach (Vector3Int tilePosition in destructiblesPositionList)
        {
            destructiblesTilemap.SetTile(tilePosition, destructiblesTile);
            Debug.Log(tilePosition);
        }
    }
    private void CharacterMovement_OnCharacterFinishMovement(object sender, System.EventArgs e)
    {
        charactersFinishedMovement++;
        if (charactersFinishedMovement >= characterArray.Length)
        {
            Debug.Log("All Characters Finished Movement");
            foreach (Character character in characterArray)
            {
                if (!character.GetCharacterMovement().IsAtGoal())
                {
                    DeactivateCharacterMovement();
                    levelHasEnded = true;
                    OnLevelLost?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
            Debug.Log("Finish Level!");
            OnLevelWon?.Invoke(this, EventArgs.Empty);
        }
    }
    private void GameInput_OnStartMovementSequence(object sender, System.EventArgs e)
    {
        //foreach (Character character in characterArray)
        //{
        //    character.GetCharacterMovement().StartMovementExecution();
        //}
        MovementManager.Instance.StartSynchronizedMovement();
    }
    private void DeactivateCharacterMovement()
    {
        foreach (Character character in characterArray)
        {
            character.GetCharacterMovement().Deactivate();
        }
    }
    private void GameInput_OnCharacterSelected(object sender, GameInput.OnCharacterSelectedEventArgs e)
    {
        if (levelHasEnded) return;
        selectedCharacter = e.selectedCharacter;

        foreach (Character character in characterArray)
        {
            character.DestroyCurrentClone();
            character.ToggleMovement(character == selectedCharacter);

            if (character != selectedCharacter && character.GetHighestStep() > 0 && selectedCharacter.GetHighestStep() > 0)
            {
                character.SpawnCloneAtStep(character.GetHighestStep() < selectedCharacter.GetHighestStep() ? 
                    character.GetHighestStep() : selectedCharacter.GetHighestStep());
            }
        }
        if (selectedCharacter.GetHighestStep() > 0) selectedCharacter.SpawnCloneAtStep(selectedCharacter.GetHighestStep());
    }

    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }
    public Character[] GetCharacterArray()
    {
        return characterArray;
    }
    public bool LevelHasEnded()
    {
        return levelHasEnded;
    }
}
