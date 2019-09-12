using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExplosionPulse : MovableObject, IRemovable, IExploder {
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
        Debug.Log(name + "(ExP) called on start");
        if (Model == null) {
            grid[X, Y].Remove(this);
            Debug.Log(name + ": No model...?");
        }
        else if (grid[X, Y].Count > 1) {
            //Explode stuff
            bool impact = false;

            //Check everything in space
            try {
                string output = "";
                foreach (EntityType obj in grid[X, Y]) {
                    output += obj.name + ", ";
                }
                Debug.Log(output);
            }
            catch {
                Debug.Log("Tile == null");
            }

            for (int i = 0; i < grid[X, Y].Count; i++) {
                if ((grid[x, y][i].collision || grid[x, y][i] as IExploder != null) && grid[x, y][i] != this) {
                    Debug.Log("Call clipping explode on " + grid[X, Y][i].name);
                    //Debug.Log(grid[X, Y][i].name + " pre-explode");
                    //Debug.Log(grid[X, Y][i].name + " post explode");
                    PushBlock pushCheck = grid[X, Y][i] as PushBlock;
                    //WIP - check if tile is free based on case statement. If true, impact = false.
                    //Check if push block
                    
                    if (pushCheck != null) {
                        switch (direction) {
                            case "up":
                                impact = !CheckMove(x, y + 1);
                                break;
                            case "down":
                                impact = !CheckMove(x, y - 1);
                                break;
                            case "left":
                                impact = !CheckMove(x - 1, y);
                                break;
                            case "right":
                                impact = !CheckMove(x + 1, y);
                                break;
                            default:
                                Debug.Log(grid[X, Y][i].name + " reported no valid path");
                                break;
                        }
                    }
                    if (pushCheck == null) {
                        impact = true;
                    }
                    grid[X, Y][i].OnExplode(direction);
                }
            }
            if (impact) {
                Debug.Log("self explode");
                OnExplode(null);
            }
        }
        else {
            Debug.Log(name + ": Doesn't detect chair - " + grid[X, Y].Count);
        }
    }

    public override bool GetDestination(string unused, bool verified) {
        if (verified) {
            newX = x;
            newY = y;
            //Debug.Log(name + ": " + newX + ", " + newY);
            switch (direction) {
                case "up":
                    if (CollideBoundary(x, y + 1)) {
                        newY += 1;
                        return true;
                    }
                    break;
                case "down":
                    if (CollideBoundary(x, y - 1)) {
                        newY -= 1;
                        return true;
                    }
                    break;
                case "left":
                    if (CollideBoundary(x - 1, y)) {
                        newX -= 1;
                        return true;
                    }
                    break;
                case "right":
                    if (CollideBoundary(x + 1, y)) {
                        newX += 1;
                        return true;
                    }
                    break;
                default:
                    break;
            }
            return false;
        }
        else {
            return base.GetDestination(direction, verified);
        }
    }

    private bool CollideBoundary(int x, int y) {
        if (x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0) {
            return false;
        }
        else {
            //Check for walls
            bool output = true;
            if (grid[x, y] != null) {
                foreach (EntityType target in grid[x, y]) {
                    if (target.collision && target as MovableObject == null) {
                        Debug.Log(target.name + " hit by impact explosion");
                        output = false;
                        target.OnExplode(direction);
                    }
                }
            }
            return output;
        }
    }

    private bool CheckMove(int x, int y) {
        if (x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0) {
            return false;
        }
        else {
            if (grid[x, y] != null) {
                foreach (EntityType obj in grid[x, y]) {
                    if (obj.collision || obj as IExploder != null) {
                        return false;
                    }
                }
            }
        }
        return true;
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

    public string Direction
    {
        get {
            return direction;
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