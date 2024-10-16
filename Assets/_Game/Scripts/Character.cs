using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterVisual characterVisual;
    [SerializeField] private CharacterClone clonePrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private CharacterClone currentClone;
    private Queue<Vector2> movementQueue = new Queue<Vector2>();
    private void Start()
    {
        GameInput.Instance.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;
        characterMovement.OnCharacterFinishMovement += CharacterMovement_OnCharacterFinishMovement;
    }

    private void CharacterMovement_OnCharacterFinishMovement(object sender, System.EventArgs e)
    {
        if (currentClone != null)
        {
            Destroy(currentClone.gameObject);
        }
    }
    private void GameInput_OnMovementKeyPressed(object sender, GameInput.OnMovementKeyPressedEventArgs e)
    {
        if (this != GameManager.Instance.GetSelectedCharacter()) return;
        movementQueue.Enqueue(e.direction);
        //if (characterCloneList[^1].IsJumping()) return;
        //CharacterClone clone = Instantiate(clonePrefab, characterCloneList.Count > 0 ? characterCloneList[^1].transform.position : transform.position, Quaternion.identity);
        //characterCloneList.Add(clone);
        //clone.OnInit(this);
        //clone.JumpToTile(e.direction);
    }
    private void Update()
    {
        if (movementQueue.Count > 0)
        {
            if (currentClone == null)
            {
                currentClone = Instantiate(clonePrefab, spriteRenderer.transform.position, Quaternion.identity);
                currentClone.OnInit(this);
            }
            currentClone.JumpToTile(movementQueue.Dequeue());
            //if (characterCloneList.Count == 0)
            //{
            //    CharacterClone clone = Instantiate(clonePrefab, spriteRenderer.transform.position, Quaternion.identity);
            //    characterCloneList.Add(clone);
            //    clone.OnInit(this);
            //    clone.JumpToTile(movementQueue.Dequeue());
            //}
            //else if (!characterCloneList[^1].IsJumping())
            //{
            //    CharacterClone clone = Instantiate(clonePrefab, characterCloneList[^1].transform.position, Quaternion.identity);
            //    characterCloneList.Add(clone);
            //    clone.OnInit(this);
            //    clone.JumpToTile(movementQueue.Dequeue());
            //}
        }
    }
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
    public SpriteRenderer GetSpriteRenderer()
    {
        return spriteRenderer;
    }
    public Vector2 GetLastClonePosition()
    {
        return currentClone.transform.position;
    }
    public void ResetCharacter()
    {
        characterVisual.PlayIdleAnimation();
        if (currentClone != null)
        {
            Destroy(currentClone.gameObject);
        }
    }
}
