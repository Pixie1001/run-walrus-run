using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public abstract class EntityType : MonoBehaviour
{
    public bool collision = true;
    public int x;
    public int y;
    protected GameObject model;
    protected List<EntityType>[,] grid;
    public AudioSource audioSource;

    public EntityType(bool collision) {
        this.collision = collision;
    }

    protected virtual void Start() {
        if (name == "Polygonal Metalon Red(Clone)(Clone)") {
            name = "Pulse " + UnityEngine.Random.value;
        }
        else if (name == "Polygonal Metalon Red(Clone)") {
            name = "Unwanted Clone";
        }
        x = (int) Math.Round(transform.position.x);
        y = (int) Math.Round(transform.position.z);
        Debug.Log("Gen " + name + " | X=" + x + " / Y=" + y);
        model = this.gameObject;
        audioSource = (AudioSource)model.AddComponent<AudioSource>();
        grid = GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().grid;
        Initialize();
    }

    protected virtual void Update() {
        //Do nothing atm
    }

    public virtual void OnExplode(string dir) {
        //Nothing happens
    }

    protected void Initialize() {
        //Debug.Log("Grid: " + grid.GetLength(0) + " / " + grid.GetLength(1));
        //Debug.Log("Init " + name + " on tile");
        if (grid[x, y] == null && x < grid.GetLength(0) && x >= 0 && y < grid.GetLength(1) && y >= 0) {
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