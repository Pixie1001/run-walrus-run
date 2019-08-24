using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectSpawner
{
    public ObjectSpawner() {

    }

    public GameObject Spawn(ObstacleType entity, int x, int y, float elevation, List<ObstacleType>[,] grid) {
        if (entity.model == null) {
            Debug.Log("no model given");
        }
        GameObject temp =  GameObject.Instantiate(entity.model, getSpawnLocation(x, y, grid.GetLength(0), grid.GetLength(1), elevation), Quaternion.identity);
        GameObject.Destroy(entity.model);
        if (temp != null) {
            if (grid[x, y] == null && x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(0) && y >= 0) {
                grid[x, y] = new List<ObstacleType> { entity };
            }
            else if (grid[x, y] != null) {
                grid[x, y].Add(entity);
            }
        }
        return temp;
    }

    public GameObject SpawnTile(GameObject model, int x, int y, float elevation, List<ObstacleType>[,] grid) {
        GameObject.Destroy(model);
        return GameObject.Instantiate(model, getSpawnLocation(x, y, grid.GetLength(0), grid.GetLength(1), elevation), Quaternion.identity);
    }

    protected Vector3 getSpawnLocation(int x, int y, float w, float h, float elevation) {
        Vector3 vector = new Vector3(0f - w / 2 + x, elevation, 0f - h / 2 + y);
        return vector;
    }
}