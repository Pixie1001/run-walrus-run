using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class ObstacleType
{
    public bool collision;
    public int x;
    public int y;
    public GameObject model;
    ObjectSpawner spawner = new ObjectSpawner();

    public ObstacleType(bool collision, GameObject model, int x, int y, float elevation, List<ObstacleType>[,] grid) {
        this.collision = collision;
        this.x = x;
        this.y = y;
        this.model = model;

        this.model = spawner.Spawn(this, x, y, elevation, grid);
    }

    public bool Explode() {
        Debug.Log("Not fragile");
        return false;
    }
}