using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int width = 10;
    public int height = 10;

    private Dictionary<Vector3Int, GameObject> 
        grid = new Dictionary<Vector3Int, GameObject>();
    private void Awake()
    {
        Instance = this;
    }
    public void RegisterEntity(GameObject entity, Vector3Int gridPos)
    {
        if (!grid.ContainsKey(gridPos))
            grid.Add(gridPos, entity);
    }
    public void UnregisterEntity(Vector3Int oldPos)
        //unregister when move off
    {
        if (grid.ContainsKey(oldPos))
            grid.Remove(oldPos);
    }
    public bool IsTileOccupied(Vector3Int gridPos)
    //TILE OCCUPIED
    {
        return grid.ContainsKey(gridPos);
    }
    public GameObject GetEntityAt(Vector3Int gridPos)
    {
        grid.TryGetValue(gridPos, out GameObject entity);
        return entity;
    }

    public bool IsInsideGrid(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < width &&
               pos.z >= 0 && pos.z < height;
    }
}
