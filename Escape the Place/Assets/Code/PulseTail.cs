using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulseTail : MovableObject, IRemovable
{
    bool terminate = false;

    public PulseTail() : base(false) {
    }


    // Use this for initialization
    protected override void Start() {
        base.Start();
        model.transform.GetChild(0).GetComponent<Animator>().Play("MOVE");
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public override void Move() {

    }

    public void Move(int nX, int nY) {
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

        //Play walking animation
        model.transform.GetChild(0).GetComponent<Animator>().Play("MOVE");

        //Move model
        start = transform.position;
        destination = new Vector3(nX, transform.position.y, nY);
        currTime = 0;

        x = nX;
        y = nY;
    }

    public override void OnExplode(string obsolete) {
        base.OnExplode(null);
    }

    public void Delete() {
        terminate = true;
        Destroy(gameObject);
    }

    public bool Terminate
    {
        get {
            return terminate;
        }
    }
}
