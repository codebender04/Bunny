using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private CharacterVisual characterVisual;
    [SerializeField] private CharacterClone clonePrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject characterDeadDummy;
    [SerializeField] private CharacterType characterType;

    private CharacterClone currentClone;
    private Queue<Vector2> movementBufferQueue = new Queue<Vector2>();
    private List<Vector2> movementList = new List<Vector2>();
    private int currentStep = 0;
    private bool isCoroutineRunning;
    private bool isDead = false;

    private void Start()
    {
        GameInput.Instance.OnMovementKeyPressed += GameInput_OnMovementKeyPressed;
        GameInput.Instance.OnCharacterSelected += GameInput_OnCharacterSelected;
        GameInput.Instance.OnStartMovementSequence += GameInput_OnStartMovementSequence;
        characterMovement.OnCharacterFinishMovement += CharacterMovement_OnCharacterFinishMovement;
        characterMovement.OnCharacterDie += CharacterMovement_OnCharacterDie;
    }

    private void GameInput_OnStartMovementSequence(object sender, System.EventArgs e)
    {
        DestroyCurrentClone();
    }

    private void GameInput_OnCharacterSelected(object sender, GameInput.OnCharacterSelectedEventArgs e)
    {
        if (e.selectedCharacter == this)
        {
            characterVisual.SelectCharacter();
        }
    }

    private void OnMouseEnter()
    {
        characterVisual.TurnOnOutline();
    }
    private void OnMouseExit()
    {
        characterVisual.TurnOffOutline();
    }

    private void CharacterMovement_OnCharacterDie(object sender, System.EventArgs e)
    {
        isDead = true;
    }

    private void CharacterMovement_OnCharacterFinishMovement(object sender, System.EventArgs e)
    {
        DestroyCurrentClone();
    }

    private void GameInput_OnMovementKeyPressed(object sender, GameInput.OnMovementKeyPressedEventArgs e)
    {
        if (this != GameManager.Instance.GetSelectedCharacter()) return;

        // Add the direction to the queue
        currentStep++;
        movementBufferQueue.Enqueue(e.direction);
        movementList.Add(e.direction);
        // If this is the first movement after selecting a character
        if (movementList.Count == 1)
        {
            // Instantiate a clone at the position of every other character
            foreach (Character character in GameManager.Instance.GetCharacterArray())
            {
                if (character != this && character.currentClone == null)
                {
                    character.currentClone = Instantiate(clonePrefab, character.transform.position, Quaternion.identity);
                    character.currentClone.OnInit(character);
                }
            }
        }
        if (!isCoroutineRunning)
        {
            StartCoroutine(MoveCloneCoroutine());
        }
    }
    private IEnumerator MoveCloneCoroutine()
    {
        isCoroutineRunning = true;

        if (movementBufferQueue.Count > 0 && currentClone == null)
        {
            currentClone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
            currentClone.OnInit(this);
        }

        while (movementBufferQueue.Count > 0)
        {
            foreach (Character character in GameManager.Instance.GetCharacterArray())
            {
                if (character != this)
                {
                    // Move the character if that character has queued movement
                    if (character.movementList.Count > 0 && movementList.Count <= character.movementList.Count)
                    {
                        StartCoroutine(character.currentClone.JumpToDirection(character.movementList[movementList.Count - 1]));
                    }
                }
            }
            yield return currentClone.JumpToDirection(movementBufferQueue.Dequeue()); // Move the clone
        }

        isCoroutineRunning = false;
    }
    public void ToggleMovement(bool isSelected)
    {
        if (isSelected)
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
        return currentClone != null ? currentClone.transform.position : (Vector2)transform.position;
    }

    public void ResetCharacter()
    {
        isDead = false;
        characterVisual.PlayIdleAnimation();
        movementList.Clear();
        characterMovement.ClearMovement();
        DestroyCurrentClone();
    }
    public void DestroyCurrentClone()
    {
        if (currentClone != null)
        {
            Destroy(currentClone.gameObject);
        }
    }
    public void SpawnCloneAtStep(int step)
    {
        step -= 1; //Because I'm passing in the number of steb and need to get the index
        if (step > movementList.Count)
        {
            Debug.LogError("Spawn clone at step greater than list!");
            return;
        }
        DestroyCurrentClone();
        currentClone = Instantiate(clonePrefab, transform.position, Quaternion.identity);
        currentClone.OnInit(this);
        Vector2 position = transform.position;
        for (int i = 0; i <= step; i++)
        {
            position += movementList[i];
        }
        currentClone.transform.position = position;
    }
    public int GetHighestStep()
    {
        return movementList.Count; 
    }
    public Vector2 GetVisualOffset()
    {
        return characterVisual.transform.localPosition;
    }
    public CharacterVisual GetCharacterVisual()
    {
        return characterVisual;
    }
    public bool IsDead()
    {
        return isDead;
    }
    public GameObject GetDeadDummy()
    {
        return characterDeadDummy;
    }

    public bool IsAtGoal()
    {
        return TileManager.Instance.HasGoalTile(characterMovement.GetCurrentCell(), characterType) || !TileManager.Instance.HasGoal(characterType);
    }
}
