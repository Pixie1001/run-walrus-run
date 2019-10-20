using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExplosionPulse : MovableObject, IRemovable, IExploder {
    public string direction;
    float deathTimer = 1.333f;
    public bool terminate;
    bool tailEnabled = false, tailTrigger = false, counterPulse = false;
    PulseTail tail;
    int tailX = 0, tailY = 0;
    AudioClip moveSE, pulseCollisionSE, hitWallSE;

    public ExplosionPulse() : base(false) {
        //model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        //model.AddComponent(Type.GetType("ExplosionPulse"));
        //Debug.Log("New EP script created");
    }

    protected override void Start() {
        base.Start();
        PushBlock pushCheck;
        model.transform.GetChild(0).GetComponent<Animator>().Play("MOVE");
        //Sound stuff
        moveSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionMove");
        pulseCollisionSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionPulseCollision");
        //pulseCollisionSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitFailState");
        hitWallSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitWallSE");

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
                    pushCheck = grid[X, Y][i] as PushBlock;
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
                    if (grid[X, Y][i] as ExplosionPulse != null && grid[X, Y][i] != this) {
                        counterPulse = true;
                    }
                    grid[X, Y][i].OnExplode(direction);
                }
            }
            if (impact) {
                Debug.Log("self explode");
                if (counterPulse) {
                    audioSource.PlayOneShot(pulseCollisionSE);
                }
                OnExplode(null);
            }
        }
        Debug.Log(name + " triggered a telegraph");
        //Get grid, find x and y
        //start loop
        //Check tile. If empty, out of bounds or Pit, end loop
        //if chair, check if there's a thing behind it, and mark 'pushing'. If pushing already true, end loop.
        //otherwise create primitive plane with TelegraphTile script (Orange).
        //Use SpawnTile to generate
        //end loop
        int checkX = X;
        int checkY = Y;
        int pushCheckX = 0;
        int pushCheckY = 0;
        bool hitChair = false;
        bool interrupted = false;
        bool outOfBounds = false;

        while (!interrupted) {
            //Check if tile is within level
            if (checkX < grid.GetLength(0) && checkX >= 0 && checkY < grid.GetLength(1) && checkY >= 0) {
                //Check if tile is occupied
                if (grid[checkX, checkY] != null) {
                    foreach (EntityType obj in grid[checkX, checkY]) {
                        //Check if path is interrupted
                        if (obj.collision) {
                            pushCheck = obj as PushBlock;
                            //Check if blocker is a pushblock
                            if (pushCheck != null && !hitChair) {
                                hitChair = true;
                                //Check if chair will buffer
                                switch (direction) {
                                    case "up":
                                        pushCheckX = checkX;
                                        pushCheckY = checkY + 1;
                                        break;
                                    case "down":
                                        pushCheckX = checkX;
                                        pushCheckY = checkY - 1;
                                        break;
                                    case "left":
                                        pushCheckX = checkX - 1;
                                        pushCheckY = checkY;
                                        break;
                                    case "right":
                                        pushCheckX = checkX + 1;
                                        pushCheckY = checkY;
                                        break;
                                    default:
                                        Debug.Log("ERROR: Invalid direction");
                                        pushCheckX = checkX;
                                        pushCheckY = checkY;
                                        break;
                                }
                                if (pushCheckX < grid.GetLength(0) && pushCheckX >= 0 && pushCheckY < grid.GetLength(1) && pushCheckY >= 0) {
                                    //Check if tile is occupied
                                    if (grid[pushCheckX, pushCheckY] != null) {
                                        foreach (EntityType obj2 in grid[pushCheckX, pushCheckY]) {
                                            //Check if path is interrupted
                                            if (obj2.collision) {
                                                interrupted = true;
                                            }
                                        }
                                    }
                                }
                                else {
                                    interrupted = true;
                                }
                            }
                            else {
                                interrupted = true;
                            }
                        }

                    }
                }
            }
            else {
                interrupted = true;
                outOfBounds = true;
            }
            if (!outOfBounds && !(interrupted && hitChair)) {
                //Generate telegraph tile
                GameObject telegraph = GameObject.CreatePrimitive(PrimitiveType.Plane);
                //TelegraphTile tileScript = new TelegraphTile();
                TelegraphTile tileScript = (TelegraphTile)telegraph.AddComponent(System.Type.GetType("TelegraphTile"));
                tileScript.enabled = true;
                if (interrupted) {
                    telegraph.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.333f, .0196f, .0f, 0.1f));
                }
                else {
                    telegraph.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0.4f, .039f, 0.1f));
                }
                telegraph.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                new ObjectSpawner().SpawnTile(telegraph, checkX, checkY, 0.02f, grid);
            }

            switch (direction) {
                case "up":
                    checkY += 1;
                    break;
                case "down":
                    checkY -= 1;
                    break;
                case "left":
                    checkX -= 1;
                    break;
                case "right":
                    checkX += 1;
                    break;
                default:
                    Debug.Log("ERROR: Invalid direction");
                    break;
            }
        }
    }

    public override void Move() {
        audioSource.PlayOneShot(moveSE, 0.3f);
        /*
        tailX = X;
        tailY = Y;
        tailTrigger = true;
        */
        base.Move();
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

    public override void OnExplode(string dir) {
        Debug.Log(name + " exploded :o");
        terminate = true;
        Model.transform.GetChild(0).GetComponent<Animator>().Play("EXPLODE");
    }

    protected override void Update() {
        base.Update();
        if (currTime >= targetTime - 0.6f && tailTrigger) {
            if (!tailEnabled) {
                tailEnabled = true;
                GameObject model = GameObject.Instantiate(Resources.Load("Prefabs/Crack_02") as GameObject);
                //Add script and animator to prefab
                model = new ObjectSpawner().SpawnTail(model, tailX, tailY, 0.03f, direction, grid);
                tail = model.GetComponent<PulseTail>();
            }
            else {
                Debug.Log("Move crack");
                tail.Move(tailX, tailY);
                //Add code for OnExplode
            }
            tailTrigger = false;
        }

        if (terminate) {
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer <= 0) {
            if (tailEnabled) {
                tail.Delete();
            }
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