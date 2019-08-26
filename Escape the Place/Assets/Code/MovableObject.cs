using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public abstract class MovableObject : EntityType
{
    float speed = OnLoad.speed;
    float targetTime = 0.5f;
    float currTime = 100;
    Vector3 start;
    Vector3 destination;

    public MovableObject(bool collision) : base (collision) {
        
    }

    public bool Move(string direction, List<EntityType>[,] grid) {
        Debug.Log("Requested move");
        switch (direction) {
            case "up":
                if (CheckMove(x, y + 1, grid)) {
                    HandleMovement(x, y, x, y + 1, grid);
                    return true;
                }
                break;
            case "down":
                if (CheckMove(x, y - 1, grid)) {
                    HandleMovement(x, y, x, y - 1, grid);
                    return true;
                }
                break;
            case "left":
                if (CheckMove(x - 1, y, grid)) {
                    HandleMovement(x, y, x - 1, y, grid);
                    return true;
                }
                break;
            case "right":
                if (CheckMove(x + 1, y, grid)) {
                    HandleMovement(x, y, x + 1, y, grid);
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

    void Update() {
        if (currTime <= targetTime * 2) {
            currTime += Time.deltaTime / targetTime;
            transform.position = Vector3.Lerp(start, destination, currTime);
        }
    }

    private bool CheckMove(int x, int y, List<EntityType>[,] grid) {
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

    private void HandleMovement(int _x, int _y, int _nX, int _nY, List<EntityType>[,] grid) {
        Debug.Log("Handling move");
        //Remove old position
        if (grid[_x, _y] == null && _x < grid.GetLength(0) && _x >= 0 && _y < grid.GetLength(0) && _y >= 0) {
            //Do nothing - no list to remove self from :D
        }
        else if (grid[_x, _y] != null) {
            grid[_x, _y].Remove(this);
        }

        //Add new position
        if (grid[_nX, _nY] == null && _nX < grid.GetLength(0) && _nX >= 0 && _nY < grid.GetLength(0) && _nY >= 0) {
            grid[_nX, _nY] = new List<EntityType> { this };
        }
        else if (grid[_nX, _nY] != null) {
            grid[_nX, _nY].Add(this);
        }
        Debug.Log("Grid Coords: " + _nX + " / " + _nY);

        //rotate model
        string dir = (_nX - _x) + "/" + (_nY - _y);
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
        destination = new Vector3(_nX, transform.position.y, _nY);
        currTime = 0;

        x = _nX;
        y = _nY;
    }
}