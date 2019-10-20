using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public abstract class MovableObject : EntityType
{
    protected float targetTime = 0.5f;
    protected float currTime = 100;
    protected Vector3 start;
    protected Vector3 destination;
    public bool moveChecked = false;
    public int newX;
    public int newY;

    public MovableObject(bool collision) : base (collision) {

    }

    protected override void Start() {
        base.Start();
        GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().movableList.Add(this);
    }

    protected override void Update() {
        base.Update();
        if (currTime <= targetTime * 2) {
            currTime += Time.deltaTime / targetTime;
            transform.position = Vector3.Lerp(start, destination, currTime);
        }
    }

    public virtual bool GetDestination(string direction, bool verified) {
        newX = x;
        newY = y;
        //Debug.Log(name + ": " + newX + ", " + newY);
        switch (direction) {
            case "up":
                if (CheckBoundary(x, y + 1)) {
                    newY += 1;
                    return true;
                }
                break;
            case "down":
                if (CheckBoundary(x, y - 1)) {
                    newY -= 1;
                    //Debug.Log(name + " (after down): " + newX + ", " + newY);
                    return true;
                }
                break;
            case "left":
                if (CheckBoundary(x - 1, y)) {
                    newX -= 1;
                    return true;
                }
                break;
            case "right":
                if (CheckBoundary(x + 1, y)) {
                    newX += 1;
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }

    public virtual void Move() {
        if (!(X == newX && Y == newY)) {
            //Remove old position
            if (grid[X, Y] == null && X < grid.GetLength(0) && X >= 0 && Y < grid.GetLength(1) && Y >= 0) {
                //Do nothing - no list to remove self from :D
            }
            else if (grid[X, Y] != null) {
                grid[X, Y].Remove(this);
            }

            //Add new position
            if (grid[newX, newY] == null && newX < grid.GetLength(0) && newX >= 0 && newY < grid.GetLength(1) && newY >= 0) {
                grid[newX, newY] = new List<EntityType> { this };
            }
            else if (grid[newX, newY] != null) {
                grid[newX, newY].Add(this);
            }

            //rotate model
            string dir = (newX - X) + "/" + (newY - Y);
            float rotate;
            switch (dir) {
                case "0/1":
                    rotate = 0f;
                    break;
                case "0/-1":
                    rotate = 180f;
                    break;
                case "1/0":
                    rotate = rotate = 90f;
                    break;
                case "-1/0":
                    rotate = -90f;
                    break;
                default:
                    Debug.Log("Invalid turn? " + name);
                    rotate = 0f;
                    break;
            }
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotate, transform.eulerAngles.z);

            //Play walking animation
            if (this as ExplosionPulse == null) {
                GetComponent<Animator>().Play("MOVE");
            }
            else {
                model.transform.GetChild(0).GetComponent<Animator>().Play("MOVE");
            }

            //Move model
            //transform.transform.Translate(0f, 0f, speed);
            start = transform.position;
            destination = new Vector3(newX, transform.position.y, newY);
            currTime = 0;

            x = newX;
            y = newY;
        }
    }

    /*
    public bool Move(string direction) {
        Debug.Log("Requested move");
        switch (direction) {
            case "up":
                if (CheckMove(x, y + 1)) {
                    HandleMovement(x, y + 1);
                    return true;
                }
                break;
            case "down":
                if (CheckMove(x, y - 1)) {
                    HandleMovement(x, y - 1);
                    return true;
                }
                break;
            case "left":
                if (CheckMove(x - 1, y)) {
                    HandleMovement(x - 1, y);
                    return true;
                }
                break;
            case "right":
                if (CheckMove(x + 1, y)) {
                    HandleMovement(x + 1, y);
                    return true;
                }
                break;
            default:
                Debug.Log("Hit wall :C");
                return false;
        }
        Debug.Log("Switch ended");
        return false;
    }
    */

    private bool CheckBoundary(int x, int y) {
        if (x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0) {
            return false;
        }
        else {
            if (grid[x, y] != null) {
                foreach (EntityType obj in grid[x, y]) {
                    if (obj.collision && collision && obj as MovableObject == null) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    /*
    protected bool CheckMove(int x, int y) {
        Debug.Log("Check movement");
        if (x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0) {
            return false;
        }
        else {
            if (grid[x, y] != null) {
                foreach (EntityType obj in grid[x, y]) {
                    if (obj.collision) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    */

    /*
    protected void HandleMovement(int nX, int nY) {
        Debug.Log("Handling move");
        //Remove old position
        if (grid[X, Y] == null && X < grid.GetLength(0) && X >= 0 && Y < grid.GetLength(0) && Y >= 0) {
            //Do nothing - no list to remove self from :D
        }
        else if (grid[X, Y] != null) {
            grid[X, Y].Remove(this);
        }

        //Add new position
        if (grid[nX, nY] == null && nX < grid.GetLength(0) && nX >= 0 && nY < grid.GetLength(0) && nY >= 0) {
            grid[nX, nY] = new List<EntityType> { this };
        }
        else if (grid[nX, nY] != null) {
            grid[nX, nY].Add(this);
        }
        Debug.Log("Grid Coords: " + nX + " / " + nY);

        //rotate model
        string dir = (nX - X) + "/" + (nY - Y);
        float rotate;
        switch (dir) {
            case "0/1":
                rotate = 0f;
                break;
            case "0/-1":
                rotate = 180f;
                break;
            case "1/0":
                rotate = rotate = 90f;
                break;
            case "-1/0":
                rotate = -90f;
                break;
            default:
                Debug.Log("Invalid turn?");
                rotate = 0f;
                break;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotate, transform.eulerAngles.z);

        //Play walking animation
        if (GetComponent<Animator>() != null) {
            GetComponent<Animator>().Play("MOVE");
        }

        //Move model
        //transform.transform.Translate(0f, 0f, speed);
        start = transform.position;
        destination = new Vector3(nX, transform.position.y, nY);
        currTime = 0;

        x = nX;
        y = nY;
    }
    */
}