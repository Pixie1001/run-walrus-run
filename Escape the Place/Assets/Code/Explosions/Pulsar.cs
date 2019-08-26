using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Pulsar : ExplosionType
{
    private int count = 0;

    public override bool Explode(Avatar avatar, List<EntityType>[,] grid) {
        Debug.Log("EXPLOSION!!!");
        int x = avatar.X;
        int y = avatar.Y;

        SpawnPulse("up", x, y + 1, grid);
        //SpawnPulse("down", x, y - 1, grid);
        //SpawnPulse("left", x - 1, y, grid);
        //SpawnPulse("right", x + 1, y, grid);

        Debug.Log("Explosion done");
        return false;
    }

    private bool SpawnPulse(string direction, int x, int y, List<EntityType>[,] grid) {
        //Check if tile is within grid
        if (x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(0) && y >= 0) {
            if (grid[x, y] != null) {
                bool clear = true;
                foreach (EntityType obj in grid[x, y]) {
                    //Check if space is occupied
                    if (obj.collision) {
                        //Handle collision - play an animation, then give results?
                        return false;
                    }
                }
            }
        }
        else {
            return false;
        }
        //Spawn thing
        GameObject model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        model.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Controllers/pulse_controller") as RuntimeAnimatorController;
        ExplosionPulse pulse = (ExplosionPulse) model.AddComponent(System.Type.GetType("ExplosionPulse"));
        count += 1;
        Debug.Log("Script added #" + count);
        pulse.Model = model;
        pulse.direction = direction;
        new ObjectSpawner().Spawn(pulse, x, y, 0.2f, direction, 0.2f, grid);
        return true;
    }

}