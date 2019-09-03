using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ObjectSpawner
{
    public ObjectSpawner() {

    }

    public GameObject Spawn(EntityType entity, int x, int y, float elevation, string facing, float scale, List<EntityType>[,] grid) {
        GameObject temp = Spawn(entity, x, y, elevation, grid);
        temp.transform.localScale = new Vector3(scale, scale, scale);
        float rotate;
        switch (facing) {
            case "up":
                rotate = 0f;
                break;
            case "down":
                rotate = 180f;
                break;
            case "left":
                rotate = rotate = -90f;
                break;
            case "right":
                rotate = 90f;
                break;
            default:
                Debug.Log("Invalid turn?");
                rotate = 0f;
                break;
        }
        temp.transform.eulerAngles = new Vector3(temp.transform.eulerAngles.x, rotate, temp.transform.eulerAngles.z);
        return temp;
    }

    public GameObject Spawn(EntityType entity, int x, int y, float elevation, List<EntityType>[,] grid) {
        if (entity.Model == null) {
        }
        GameObject temp = GameObject.Instantiate(entity.Model, getSpawnLocation(x, y, grid.GetLength(0), grid.GetLength(1), elevation), Quaternion.identity);
        GameObject.Destroy(entity.Model);
        return temp;
    }

    public GameObject SpawnTile(GameObject model, int x, int y, float elevation, List<EntityType>[,] grid) {
        GameObject.Destroy(model);
        return GameObject.Instantiate(model, getSpawnLocation(x, y, grid.GetLength(0), grid.GetLength(1), elevation), Quaternion.identity);
    }

    protected Vector3 getSpawnLocation(int x, int y, float w, float h, float elevation) {
        //Vector3 vector = new Vector3(0f - w / 2 + x, elevation, 0f - h / 2 + y);
        Vector3 vector = new Vector3(0f + x, elevation, 0f + y);
        return vector;
    }
}