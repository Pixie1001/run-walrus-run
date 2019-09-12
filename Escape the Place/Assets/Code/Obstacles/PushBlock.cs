using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PushBlock : MovableObject
{

    public PushBlock() : base(true) {

    }

    public override bool GetDestination(string unused, bool verified) {
        newX = X;
        newY = Y;
        return true;
    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        //Debug.Log(name + " exploded towards " + dir + " newX=" + newX + ", newY=" + newY);
        Move(dir);
    }

    private bool Move(string direction) {
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
        return false;
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

    private void HandleMovement(int nX, int nY) {
        //Remove old position
        if (grid[X, Y] == null && X < grid.GetLength(0) && X >= 0 && Y < grid.GetLength(1) && Y >= 0) {
            //Do nothing - no list to remove self from :D
        }
        else if (grid[X, Y] != null) {
            grid[X, Y].Remove(this);
        }

        //Add new position
        if (grid[nX, nY] == null && nX < grid.GetLength(0) && nX >= 0 && nY < grid.GetLength(1) && nY >= 0) {
            grid[nX, nY] = new List<EntityType> { this };
        }
        else if (grid[nX, nY] != null) {
            grid[nX, nY].Add(this);
        }
        //Debug.Log("Grid Coords: " + nX + " / " + nY);

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
                Debug.Log("Invalid turn? (PB)");
                rotate = 0f;
                break;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotate, transform.eulerAngles.z);

        //Play walking animation
        if (GetComponent<Animator>() != null) {
            GetComponent<Animator>().Play("MOVE");
        }

        //Move model
        start = transform.position;
        destination = new Vector3(nX, transform.position.y, nY);
        currTime = 0;

        x = nX;
        y = nY;

        //Debug.Log("New Coords: " + X + " / " + Y);
    }

    public override void Move() {
        //do nothing
    }

}
