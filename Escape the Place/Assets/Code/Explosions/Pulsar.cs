using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Pulsar : ExplosionType
{
    private ObjectSpawner spawner = new ObjectSpawner();
    private int count = 0;

    public override bool Explode(Avatar avatar, List<EntityType>[,] grid) {
        int x = avatar.X;
        int y = avatar.Y;

        //SpawnPulse("up", x, y + 1, grid);
        SpawnPulse("down", x, y - 1, grid);
        //SpawnPulse("left", x - 1, y, grid);
        //SpawnPulse("right", x + 1, y, grid);

        return false;
    }

    private bool SpawnPulse(string direction, int x, int y, List<EntityType>[,] grid) {
        //Check if tile is within grid
        if (x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(1) && y >= 0) {
            if (grid[x, y] != null) {
                foreach (EntityType obj in grid[x, y]) {
                    //Check if space is occupied
                    if (obj as Pit != null) {
                        return false;
                    }
                }
            }
        }
        else {
            return false;
        }
        //Spawn thing
        //GameObject model = GameObject.Instantiate(Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject);
        GameObject model = GameObject.Instantiate(Resources.Load("Prefabs/Crack_01") as GameObject);
        //model.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Controllers/pulse_controller") as RuntimeAnimatorController;
        ExplosionPulse pulse = (ExplosionPulse) model.AddComponent(System.Type.GetType("ExplosionPulse"));
        pulse.Model = model;
        pulse.direction = direction;
        //spawner.Spawn(pulse, x, y, 0.2f, direction, 0.2f, grid);
        spawner.Spawn(pulse, x, y, 0.03f, direction, null, grid);
        return true;
    }

}