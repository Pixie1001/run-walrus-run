using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TileGrid : MonoBehaviour
{
    //List<List<ObstacleType>> tiles = new List<List<ObstacleType>>();
    ObjectSpawner spawner = new ObjectSpawner();
    public List<ObstacleType>[,] tiles;
    Avatar avatar;
    public int width = 10;
    public int height = 10;
    public GameObject avModel;

    void Start() {
        GenerateLevel();
    }

    void Update() {
        {
            if (Input.GetKeyUp(KeyCode.W)) { // W
                avatar.PlayerMove("up", tiles);
            }
            if (Input.GetKeyUp(KeyCode.A)) { // A
                avatar.PlayerMove("left", tiles);
            }
            if (Input.GetKeyUp(KeyCode.S)) { // S
                avatar.PlayerMove("down", tiles);
            }
            if (Input.GetKeyUp(KeyCode.D)) { // D
                avatar.PlayerMove("right", tiles);
            }
        }
    }

    private void GenerateLevel() {
        tiles = new List<ObstacleType>[width, height];
        avModel.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Controllers/player_controller") as RuntimeAnimatorController;

        //Temp - remove after IdleChan is repalced
        avModel.GetComponent<UnityChan.IdleChanger>().enabled = false;
        avModel.GetComponent<UnityChan.FaceUpdate>().enabled = false;

        avModel = Instantiate(avModel);
        avatar = new Avatar(9, 9, -0.5f, avModel, new Pulsar(), 3, tiles);
        //avatar = new Avatar(9, 9, GameObject.FindWithTag("Player"), new Pulsar(), 3, tiles);
        //List<ObstacleType> obstacles = new List<ObstacleType>();
        Material m1 = Resources.Load("Friends", typeof(Material)) as Material;
        Material m2 = null;

        RenderFloor(m1, m2);
        
        //Place Obstacles
        new Wall(3, 2, 0, GameObject.FindWithTag("Wall"), tiles);
        new Wall(5, 7, 0, GameObject.FindWithTag("Wall"), tiles);
    }

    protected void RenderFloor (Material m1, Material m2) {
        int xCount = 0;
        int yCount = 0;
        bool odd = false;
        GameObject nBlock;
        GameObject block;

        //Define floor blocks
        GameObject block1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(block1);
        if (m1 != null) {
            block1.GetComponent<Renderer>().material = m1;
        }
        else {
            block1.GetComponent<Renderer>().materials[0].color = Color.white;
        }

        GameObject block2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Destroy(block2);
        if (m2 != null) {
            block2.GetComponent<Renderer>().material = m2;
        }
        else {
            block2.GetComponent<Renderer>().materials[0].color = Color.grey;
        }

        //Render floor
        while (xCount < width && yCount < height) {
            if (odd) {
                block = block1;
                odd = false;
            }
            else {
                block = block2;
                odd = true;
            }
            block.name = "Tile " + xCount + "/" + yCount;
            nBlock = spawner.SpawnTile(block, xCount, yCount, -1f, tiles);

            xCount += 1;
            if (xCount >= width) {
                xCount = 0;
                yCount += 1;
                if (odd) {
                    odd = false;
                }
                else {
                    odd = true;
                }
            }
        }
    }
}