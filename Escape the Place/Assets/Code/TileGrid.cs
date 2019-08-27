using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TileGrid : MonoBehaviour
{
    //List<List<ObstacleType>> tiles = new List<List<ObstacleType>>();
    ObjectSpawner spawner = new ObjectSpawner();
    public List<EntityType>[,] tiles;
    public List<EntityType> movableList;
    public bool loseState = false;
    Avatar avatar;
    List<string> inputLog;
    public int width = 10;
    public int height = 10;
    public GameObject avModel;
    public bool renderTiles;
    int[] goal;

    void Awake() {
        Debug.Log("Level starto");
        GenerateLevel();
    }

    void Update() {
        //Check for inputs
        if (!loseState) {
            if (Input.GetKeyUp(KeyCode.W)) { // W
                ProcessTurn("up");
            }
            if (Input.GetKeyUp(KeyCode.A)) { // A
                ProcessTurn("left");
            }
            if (Input.GetKeyUp(KeyCode.S)) { // S
                ProcessTurn("down");
            }
            if (Input.GetKeyUp(KeyCode.D)) { // D
                ProcessTurn("right");
            }
        }
    }

    private void ProcessTurn(string pDirection) {
        //Get each MovableObject to run a method that calcs their nX and nY (Don't change it if they hit a level boundary - some might also explode here)
        //Get nX and nY of each movable object
        //Check each object's nX and nY against each other to see if any would collide.
        //If both objects are not ExplosionPulses, they remain static (Have hteir nX and nY set to their base X and Y vals). ExplosionPulses move and explode, hitting the other object which moves as normal.
        //If player isn't stopped by this check, call HandleMovement for the objects that can move (HandleMovement will need a validator checks if nX and nY have actally changed)
        //Then spawn extra explosions, starting on player's tile, and have them move 1 space.

        //Get destination
        foreach (MovableObject obj in movableList) {
            obj.GetDestination(pDirection);
        }
        //Adjust destination based on collision
        foreach (MovableObject obj in movableList) {
            foreach (MovableObject comp in movableList) {
                if (obj.newY == comp.newY && obj.newX == comp.newX) {
                    if (obj.collision && comp.collision) {
                        obj.newX = obj.X;
                        obj.newY = obj.Y;
                        comp.newX = comp.X;
                        comp.newY = comp.Y;
                    }
                }
            }
        }
        //Check if player's move was valid
        if (!(avatar.newX == avatar.X && avatar.newY == avatar.Y)) {
            //Move all objects to new coords
            foreach (MovableObject obj in movableList) {
                obj.Move();
            }
            //Check win state
            if (avatar.X == goal[0] && avatar.Y == goal[1]) {
                Debug.Log("You win!!!");
            }
            else if (loseState) {
                Debug.Log("You lose :(");
            }
            //Dispose of exploded objects
            IRemovable temp;
            for (int i = 0; i < movableList.Count; i++) {
                temp = movableList[i] as IRemovable;
                if (temp != null) {
                    if (temp.Terminate) {
                        movableList.Remove(movableList[i]);
                    }
                }
            }
        }
        else {
            avatar.Rotate(pDirection);
        }
    }

    private void GenerateLevel() {
        if (GameObject.FindWithTag("End") != null) {
            goal = new int[2] { (int)GameObject.FindWithTag("End").transform.position.x, (int)GameObject.FindWithTag("End").transform.position.z };
        }
        else {
            goal = new int[2] { 0, 0 };
            Debug.Log("Please add an 'End' tile to this level");
        }
        Debug.Log("Gen level");
        tiles = new List<EntityType>[width, height];
        avModel = GameObject.FindWithTag("Avatar");
        avModel.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Controllers/player_controller") as RuntimeAnimatorController;

        //Temp - remove after IdleChan is repalced
        //avModel.GetComponent<UnityChan.IdleChanger>().enabled = false;
        //avModel.GetComponent<UnityChan.FaceUpdate>().enabled = false;

        //avModel = Instantiate(avModel);
        //avatar = new Avatar(9, 9, -0.5f, avModel, new Pulsar(), 3, tiles);
        //avatar = new Avatar(9, 9, GameObject.FindWithTag("Player"), new Pulsar(), 3, tiles);
        avatar = GameObject.FindWithTag("Avatar").GetComponent<Avatar>();
        //List<EntityType> obstacles = new List<EntityType>();
        Material m1 = Resources.Load("Friends", typeof(Material)) as Material;
        Material m2 = null;

        if (renderTiles) {
            RenderFloor(m1, m2);
        }
        
        //Place Obstacles
        //new Wall(3, 2, 0, GameObject.FindWithTag("Wall"), tiles);
        //new Wall(5, 7, 0, GameObject.FindWithTag("Wall"), tiles);
    }

    protected void RenderFloor (Material m1, Material m2) {
        if (GameObject.FindGameObjectWithTag("TempGrid") != null) {
            Destroy(GameObject.FindGameObjectWithTag("TempGrid"));
        }

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
            nBlock = spawner.SpawnTile(block, xCount, yCount, -0.5f, tiles);

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