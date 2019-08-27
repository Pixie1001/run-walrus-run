using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExplosionPulse : MovableObject, IRemovable {
    public string direction;
    int nY;
    int nX;
    float deathTimer = 1.333f;
    public bool terminate;

    public ExplosionPulse() : base(false) {
        //model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        //model.AddComponent(Type.GetType("ExplosionPulse"));
        //Debug.Log("New EP script created");
    }

    public override bool GetDestination(string unused) {
        bool output = base.GetDestination(direction);
        if (!output) {
            OnExplode(null);
        }
        return output;
    }

    /*
     * //FIX this up later to handle explosions - be careful about how it functions, some checks might need to be done via ProcessTurn
    public void Move() {
        if (Move(direction)) {
            //pulse moves, everyone is happy
        }
        else {
            nX = X;
            nY = Y;
            switch (direction) {
                case "up":
                    nY += 1;
                    break;
                case "down":
                    nY -= 1;
                    break;
                case "left":
                    nX -= 1;
                    break;
                case "right":
                    nX += 1;
                    break;
                default:
                    Debug.Log("Invalid direction string :C");
                    break;
            }
            //Check if pulse is leaving level
            if (nX < grid.GetLength(0) && nX >= 0 && nY < grid.GetLength(0) && nY >= 0) {
                OnExplode(null);
            }
            else {
                if (grid[nX, nY] != null) {
                    foreach (EntityType obj in grid[nX, nY]) {
                        obj.OnExplode(direction);
                    }
                }
                HandleMovement(nX, nY);
                OnExplode(null);
            }
        }
    }
    */

    public override void OnExplode(string dir) {
        terminate = true;
        foreach (List<EntityType> tile in grid) {
            if (tile != null) {
                for (int i = 0; i < tile.Count; i++) {
                    if (tile[i] == this) {
                        tile.Remove(this);
                    }
                }
            }
        }
        Model.GetComponent<Animator>().Play("EXPLODE");
    }

    protected override void Update() {
        base.Update();
        if (terminate) {
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer <= 0) {
            GameObject.Destroy(Model);
        }
    }

    public bool Terminate
    {
        get {
            return terminate;
        }
    }

    public new GameObject Model
    {
        get {
            return model;
        }
        set {
            model = value;
        }
    }

}