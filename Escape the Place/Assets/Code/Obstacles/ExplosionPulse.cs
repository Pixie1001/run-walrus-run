using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExplosionPulse : MovableObject, IRemovable {
    public string direction;
    float deathTimer = 1.333f;
    public bool terminate;

    public ExplosionPulse() : base(false) {
        //model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        //model.AddComponent(Type.GetType("ExplosionPulse"));
        //Debug.Log("New EP script created");
    }

    protected override void Start() {
        base.Start();
        if (Model == null) {
            grid[X, Y].Remove(this);
        }
        if (grid[X, Y] != null && Model != null) {
            if (grid[X, Y].Count > 1) {
                string output = "Tile " + X + "/" + Y + ": ";
                foreach (EntityType obj in grid[X, Y]) {
                    output += obj.name + ", ";
                }
                Debug.Log(name + ": " + output);
                for (int i = 0; i < grid[X, Y].Count; i++) {
                    if (grid[X, Y][i].Model != null) {
                        Debug.Log("Call impact explode on " + grid[X, Y][i].name);
                        grid[X, Y][i].OnExplode(direction);
                    }
                    else {
                        grid[X, Y].Remove(grid[X, Y][i]);
                    }
                }
                Debug.Log("Call spawn explode");
                OnExplode(null);
            }
        }
    }

    public override bool GetDestination(string unused) {
        bool output = base.GetDestination(direction);
        if (!output) {
            Debug.Log("Exploded against level edge");
            OnExplode(null);
        }
        return output;
    }

    //FIX this up later to handle explosions - be careful about how it functions, some checks might need to be done via ProcessTurn
    /*
    public override void Move() {
        base.Move();
        //Check if pulse is leaving level
        if (newX < grid.GetLength(0) && nX >= 0 && nY < grid.GetLength(0) && nY >= 0) {
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
    */

    public override void OnExplode(string dir) {
        Debug.Log(name + " exploded :o");
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
            //Model.transform.Translate(100, -100, 100);
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