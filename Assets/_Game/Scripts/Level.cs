using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Level : MonoBehaviour
{
    public static Level Instance;


    [SerializeField] private Tilemap walkableTilemap;
    [SerializeField] private Tilemap decorativesTilemap;
    [SerializeField] private Tilemap destructiblesTilemap;
    [SerializeField] private Tilemap goalSignalsTilemap;
    [SerializeField] private Tile goalSignalTile;
    [SerializeField] private SelfDestruct brokenTilePrefab;
    private Vector3[] characterPositionArray;

    private Animator animator;
    private void Start()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        characterPositionArray = new Vector3[GameManager.Instance.GetCharacterArray().Length];
        for (int i = 0; i < characterPositionArray.Length; i++)
        {
            characterPositionArray[i] = GameManager.Instance.GetCharacterArray()[i].transform.position;
        }
    }
    public Vector3[] GetStartingCharacterPositionArray()
    {
        return characterPositionArray;
    }
    public void FadeOut()
    {
        animator.SetTrigger(Constant.ANIM_FADEOUT);
    }
    public void CheckBrokenTile(CharacterMovement characterMovement)
    {
        if (destructiblesTilemap.HasTile(characterMovement.GetCurrentCell())) //Broken Tile
        {
            destructiblesTilemap.SetTile(characterMovement.GetCurrentCell(), null);
            Instantiate(brokenTilePrefab, characterMovement.GetCurrentCell() + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        }
    }
    public void CheckSignalTile(CharacterMovement characterMovement)
    {
        if (goalSignalsTilemap.HasTile(characterMovement.GetCurrentCell()))
        {
            //goalSignalsTilemap.SetTile(characterMovement.GetCurrentCell(), goalSignalTile);
            //GameManager.Instance.ActivateSignal();
            //Instantiate(goalSignalPrefab, characterMovement.GetCurrentCell() + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);

            //Vector3Int tilePosition = goalSignalsTilemap.WorldToCell(characterMovement.transform.position);
            //TileBase tile = goalSignalsTilemap.GetTile(tilePosition);

            //if (tile is ToggleTile toggleTile)
            //{
            //    toggleTile.ToggleState(tilePosition, goalSignalsTilemap);
            //    goalSignalsTilemap.RefreshTile(tilePosition);
            //}
            //Vector3Int tilePosition = goalSignalsTilemap.WorldToCell(characterMovement.transform.position);
            //Debug.Log(goalSignalsTilemap.GetInstantiatedObject(tilePosition));
        }
        if (TileManager.Instance.HasGoalSignalTile(characterMovement.GetCurrentCell()))
        {
            TileManager.Instance.ToggleTileState(characterMovement.GetCurrentCell());
        }
    }
}
