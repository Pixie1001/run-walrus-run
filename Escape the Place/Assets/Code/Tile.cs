using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Tile : MonoBehaviour
{
    private int x;
    private int y;

    private bool[,] tiles;

    public Tile ()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        x = (int)transform.position.x;
        y = (int)transform.position.z;
        //tiles = GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().tiles;
       //tiles[x, y] = true;
    }

}
