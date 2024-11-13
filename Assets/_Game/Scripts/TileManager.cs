using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager Instance;
    [SerializeField] private Tilemap tilemap;
    private Dictionary<Vector3Int, GoalSignalTile> goalSignalTileDict = new Dictionary<Vector3Int, GoalSignalTile>();
    private Dictionary<Vector3Int, GoalTile> goalTileDict = new Dictionary<Vector3Int, GoalTile>();
    private int noOfSignals = 0;
    private void Awake()
    {
        Instance = this;
    }
    public void AddGoalSignalTile(Vector3 worldPosition, GoalSignalTile tile)
    {
        //Only use for initialization
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        if (!goalSignalTileDict.ContainsKey(cellPosition))
        {
            goalSignalTileDict[cellPosition] = tile;
            noOfSignals++;
        }
    }
    public void AddGoalTile(Vector3 worldPosition, GoalTile tile)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        if (!goalTileDict.ContainsKey(cellPosition))
        {
            goalTileDict[cellPosition] = tile;
        }
    }
    public bool HasGoalSignalTile(Vector3Int cellPosition)
    {
        return goalSignalTileDict.ContainsKey(cellPosition);
    }
    public bool HasAnyGoalSignalTile()
    {
        return goalSignalTileDict.Count > 0;
    }
    public bool HasGoalTile(Vector3Int cellPosition, CharacterType type)
    {
        return goalTileDict.ContainsKey(cellPosition) && goalTileDict[cellPosition].GetCharacterType() == type
            && goalTileDict[cellPosition].IsUnlocked();
    }
    public bool HasGoal(CharacterType type)
    {
        bool hasGoal = false;
        foreach (var goalTile in goalTileDict.Values)
        {
            if (goalTile.GetCharacterType() == type)
            {
                hasGoal = true;
            }
        }
        return hasGoal;
    }
    public GoalSignalTile GetGoalSignalTile(Vector3Int cellPosition)
    {
        return goalSignalTileDict[cellPosition];
    }
    public void ToggleTileState(Vector3Int cellPosition)
    {
        if (HasGoalSignalTile(cellPosition))
        {
            goalSignalTileDict[cellPosition].ToggleState();
        }
        int activatedSignal = 0;
        foreach (GoalSignalTile tile in goalSignalTileDict.Values)
        {
            if (tile.IsActivated())
            {
                activatedSignal++;
            }
        }
        foreach (GoalTile tile in goalTileDict.Values)
        {
            if (activatedSignal == noOfSignals)
            {
                tile.Unlock();
                Debug.Log("Unlock Goal");
            }
            else
            {
                tile.Lock();
            }
        }
    }
    public void ResetAllTileState()
    {
        foreach (GoalSignalTile goalSignalTile in goalSignalTileDict.Values)
        {
            goalSignalTile.ResetState();
        }
        foreach (GoalTile goalTile in goalTileDict.Values)
        {
            goalTile.ResetState();
        }
    }
}
