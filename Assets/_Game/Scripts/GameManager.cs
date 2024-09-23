using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Character[] characterArray;
    private Character selectedCharacter;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
        GameInput.Instance.OnStartMovementSequence += GameInput_OnStartMovementSequence;
    }

    private void GameInput_OnStartMovementSequence(object sender, System.EventArgs e)
    {
        foreach (Character character in characterArray)
        {
            character.GetComponent<CharacterMovement>().StartMovementExecution();
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
