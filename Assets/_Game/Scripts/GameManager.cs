using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Character[] characterArray;
    private int charactersFinishedMovement = 0;
    private Character selectedCharacter;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
        GameInput.Instance.OnStartMovementSequence += GameInput_OnStartMovementSequence;

        foreach (Character character in characterArray)
        {
            character.GetCharacterMovement().OnCharacterFinishMovement += CharacterMovement_OnCharacterFinishMovement;
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
                    return;
                }
            }
            Debug.Log("Finish Level!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
        }
    }
    private void GameInput_OnStartMovementSequence(object sender, System.EventArgs e)
    {
        foreach (Character character in characterArray)
        {
            character.GetCharacterMovement().StartMovementExecution();
        }
    }

    private void GameInput_OnCharacterSelected(object sender, GameInput.OnCharacterSelectedEventArgs e)
    {
        selectedCharacter = e.selectedCharacter;
        foreach (Character character in characterArray)
        {
            character.ToggleMovement(selectedCharacter);
        }
    }
    public Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }
}
