using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    public void ToggleMovement(Character selectedCharacter)
    {
        if (selectedCharacter == this)
        {
            characterMovement.Activate();
        }
        else
        {
            characterMovement.Deactivate();
        }
    }
    public CharacterMovement GetCharacterMovement()
    {
        return characterMovement;
    }
}
