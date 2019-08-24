using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public abstract class MovableObject : ObstacleType
{
    float speed = OnLoad.speed;
    public int x;
    public int y;
    ObjectSpawner spawner = new ObjectSpawner();

    public MovableObject(bool collision, GameObject model, int x, int y, float elevation, List<ObstacleType>[,] grid) : base (true, model, x, y, elevation, grid) {
        this.x = x;
        this.y = y;        
    }

    public bool Move(string direction, List<ObstacleType>[,] grid) {
        if (model == null) {
            Debug.Log("Cannot find model :C");
        }
        Debug.Log("Requested move");
        switch (direction) {
            case "up":
                if (CheckMove(x, y + 1, grid)) {
                    HandleMovement(x, y, x, y + 1, grid);
                }
                break;
            case "down":
                if (CheckMove(x, y - 1, grid)) {
                    HandleMovement(x, y, x, y - 1, grid);
                }
                break;
            case "left":
                if (CheckMove(x - 1, y, grid)) {
                    HandleMovement(x, y, x - 1, y, grid);
                }
                break;
            case "right":
                if (CheckMove(x + 1, y, grid)) {
                    HandleMovement(x, y, x + 1, y, grid);
                }
                break;
            default:
                Debug.Log("Hit wall :C");
                return false;
        }
        Debug.Log("Switch ended");
        return true;
    }

    private bool CheckMove(int x, int y, List<ObstacleType>[,] grid) {
        Debug.Log("Check movement");
        if (x >= grid.GetLength(0) || x < 0 || y >= grid.GetLength(1) || y < 0) {
            return false;
        }
        else {
            if (grid[x, y] != null) {
                foreach (ObstacleType ob in grid[x, y]) {
                    if (ob.collision) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private void HandleMovement(int _x, int _y, int _nX, int _nY, List<ObstacleType>[,] grid) {
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
            grid[_nX, _nY] = new List<ObstacleType> { this };
        }
        else if (grid[_nX, _nY] != null) {
            grid[_nX, _nY].Add(this);
        }

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
        model.transform.eulerAngles = new Vector3(model.transform.eulerAngles.x, rotate, model.transform.eulerAngles.z);

        //Play walking animation
        if (model.GetComponent<Animator>() != null) {
            model.GetComponent<Animator>().Play("MOVE");
        }

        //Move model
        model.transform.transform.Translate(0f, 0f, speed);

        x = _nX;
        y = _nY;
    }
}