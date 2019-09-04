using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class EntityType : MonoBehaviour
{
    public bool collision;
    public int x;
    public int y;
    protected GameObject model;
    protected List<EntityType>[,] grid;
    protected bool loseState;
    //ObjectSpawner spawner = new ObjectSpawner();

    public EntityType(bool collision) {
        this.collision = collision;
        //this.model = spawner.Spawn(this, x, y, elevation, grid);
    }

    protected virtual void Start() {
        if (name == "Polygonal Metalon Red(Clone)(Clone)") {
            name = "Pulse " + Random.value;
        }
        else if (name == "Polygonal Metalon Red(Clone)") {
            name = "Unwanted Clone";
        }
        x = (int)transform.position.x;
        y = (int)transform.position.z;
        Debug.Log("Gen " + name + " | X=" + x + " / Y=" + y);
        model = this.gameObject;
        grid = GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().grid;
        loseState = GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().loseState;
        Initialize();
    }

    protected virtual void Update() {
        //Do nothing atm
    }

    public virtual void OnExplode(string dir) {
        //Nothing happens
    }

    private void Initialize() {
        //Debug.Log("Grid: " + grid.GetLength(0) + " / " + grid.GetLength(1));
        if (grid[x, y] == null && x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(0) && y >= 0) {
            grid[x, y] = new List<EntityType> { this };
        }
        else if (grid[x, y] != null) {
            grid[x, y].Add(this);
        }
    }

    //Attributes
    public int X
    {
        get {
            return x;
        }
    }

    public int Y
    {
        get {
            return y;
        }
    }

    public GameObject Model
    {
        get {
            return model;
        }
    }
}