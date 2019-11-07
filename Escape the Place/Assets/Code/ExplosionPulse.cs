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
    AudioClip moveSE, hitWallSE;
    private List<TelegraphTile> telegraphList;
    private List<TelegraphData> telegraphQueue;
    private float telegraphTimer = 0f;
    private int telegraphCount = 0;

    public ExplosionPulse() : base(false) {
        //model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        //model.AddComponent(Type.GetType("ExplosionPulse"));
        //Debug.Log("New EP script created");
        telegraphList = new List<TelegraphTile>();
        telegraphQueue = new List<TelegraphData>();
    }

    protected override void Start() {
        base.Start();
        ObjectSpawner spawner = new ObjectSpawner();
        PushBlock pushCheck;
        bool impact = false;
        bool red = false;
        //model.transform.GetChild(0).GetComponent<Animator>().Play("MOVE");
        model.GetComponent<Animator>().Play("MOVE");
        //Sound stuff
        moveSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionMove");
        //pulseCollisionSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionPulseCollision");
        //pulseCollisionSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitFailState");
        hitWallSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitWallSE");

        Debug.Log(name + "(ExP) called on start");
        if (Model == null) {
            grid[X, Y].Remove(this);
            Debug.Log(name + ": No model...?");
        }
        else if (grid[X, Y].Count > 1) {
            //Explode stuff
            impact = false;

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
        }
        Debug.Log(name + " triggered a telegraph");
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
                                if (obj as Fragile != null) {
                                    red = true;
                                }
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
                Color telegraphColor;
                if (red) {
                    telegraphColor = new Color(254f/255f, 104f/255f, 59f/255f);
                    telegraphQueue.Add(new TelegraphData(checkX, checkY, telegraphColor));
                }
                else if (!red && !interrupted) {
                    telegraphColor = new Color(93f/255f, 255f/255f, 249f/255f);
                    telegraphQueue.Add(new TelegraphData(checkX, checkY, telegraphColor));
                }
                //telegraph.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //telegraphList.Add(spawner.SpawnTile(telegraph, checkX, checkY, 0.02f, grid).);
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
        if (impact) {
            Debug.Log("self explode");
            /*
            if (counterPulse) {
                Debug.Log("Counterpulse");
                audioSource.PlayOneShot(pulseCollisionSE, OnLoad.sfx);
            }
            */
            OnExplode(null);
            Model.transform.GetChild(0).gameObject.SetActive(false);
            telegraphQueue.Clear();
        }
    }

    public override void Move() {
        audioSource.PlayOneShot(moveSE, 0.3f);
        for (int i = 0; i < grid[X, Y].Count; i++) {
            if (grid[X, Y][i] != null) {
                for (int c = 0; c < telegraphList.Count; c++) {
                    if (telegraphList[c] == grid[X, Y][i]) {
                        telegraphList.Remove(grid[X, Y][i] as TelegraphTile);
                        //grid[X, Y][i].gameObject.SetActive(false);
                        grid[X, Y][i].gameObject.transform.position = new Vector3(0, 0, 1000);
                        GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().TelegraphPool.Add(grid[X, Y][i].gameObject);
                        Destroy(grid[X, Y][i].gameObject.GetComponent<TelegraphTile>());
                    }
                }
            }
        }
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
        //Model.transform.GetChild(0).GetComponent<Animator>().Play("EXPLODE");
        Model.GetComponent<Animator>().Play("EXPLODE");
        for (int c = 0; c < telegraphList.Count; c++) {
            telegraphList[c].gameObject.transform.position = new Vector3(0, 0, 1000);
            GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().TelegraphPool.Add(telegraphList[c].gameObject);
            TelegraphTile temp = telegraphList[c];
            //telegraphList.Remove(temp);
            Destroy(temp);
        }
    }

    protected override void Update() {
        base.Update();
        if (telegraphCount < telegraphQueue.Count) {
            telegraphTimer += Time.deltaTime;
        }

        if (telegraphCount < telegraphQueue.Count && telegraphTimer >= 0.25f) {
            //telegraphQueue[telegraphCount]; - spawn a thing
            TelegraphTile tileScript = new ObjectSpawner().SpawnTile(telegraphQueue[telegraphCount].x, telegraphQueue[telegraphCount].y, 0.02f, grid).AddComponent<TelegraphTile>();
            tileScript.gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().color = telegraphQueue[telegraphCount].color;
            telegraphList.Add(tileScript);
            telegraphCount += 1;
            telegraphTimer = 0f;
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