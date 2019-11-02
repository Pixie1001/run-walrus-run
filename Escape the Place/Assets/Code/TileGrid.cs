using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System;

public class TileGrid : MonoBehaviour
{
    //List<List<ObstacleType>> tiles = new List<List<ObstacleType>>();
    ObjectSpawner spawner = new ObjectSpawner();
    public List<EntityType>[,] grid;
    public List<EntityType> movableList;
    public int targetSteps = 0;
    bool loseState = false, winState = false;
    Avatar avatar;
    List<string> inputLog;
    int width = 0;
    int height = 0;
    int[] goal;
    int steps = 0;
    int levelId;
    [HideInInspector] public bool pause = false;
    public GameObject[,] tiles;
    AudioClip characterHitWallSE, explosionHitWallSE, explosionPulseCollisionSE, chairStuckSE;
    private GameObject failScreen, finishScreen, finishButton, walrusIcon, gold, silver, bronze;
    AudioSource audio;
    public List<GameObject> TelegraphPool;

    [HideInInspector]
    public bool explodePause = false;

    float turnTimer = 0f;

    void Awake() {
        //Get GameObjects
        failScreen = GameObject.FindGameObjectWithTag("failScreen");
        failScreen.SetActive(false);
        finishScreen = GameObject.FindGameObjectWithTag("InGameMenu").transform.GetChild(5).gameObject.transform.GetChild(0).gameObject;
        walrusIcon = GameObject.FindGameObjectWithTag("InGameMenu").transform.GetChild(5).gameObject.transform.GetChild(1).gameObject;
        finishButton = GameObject.FindGameObjectWithTag("InGameMenu").transform.GetChild(5).gameObject.transform.GetChild(2).gameObject;
        finishScreen.SetActive(false);
        avatar = GameObject.FindWithTag("Avatar").GetComponent<Avatar>();

        audio = (AudioSource)gameObject.AddComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Audio/BGM_1");
        audio.loop = true;
        audio.Play();
        audio.volume = 0.25f;

        explosionPulseCollisionSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionPulseCollision");
        characterHitWallSE = Resources.Load<AudioClip>("Audio/Upload/CharacterWalkIntoObject");
        explosionHitWallSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitWall");
        chairStuckSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionChairCantMove");

        //Set up object pool
        for (int i = 0; i < 30; i++) {
            //GameObject objRef = Instantiate(Resources.Load("Prefabs/TelegraphTest") as GameObject);
            //objRef.SetActive(false);
            //TelegraphPool.Add(objRef);
            TelegraphPool = new List<GameObject>(GameObject.FindGameObjectsWithTag("Telegraph"));
        }

        //Find goal
        if (GameObject.FindWithTag("End") != null) {
            goal = new int[2] { (int)Math.Round(GameObject.FindWithTag("End").transform.position.x), (int)Math.Round(GameObject.FindWithTag("End").transform.position.z) };
        }
        else {
            goal = new int[2] { 0, 0 };
            Debug.Log("Please add an 'End' tile to this level");
        }

        //Set base steps value
        try {
            GameObject display = GameObject.FindGameObjectWithTag("UI (Steps)");
            display.GetComponent<Text>().text = 0.ToString();
        }
        catch {
            Debug.Log("Error: No UI element deteted");
        }

        //Set base steps value
        try {
            gold = GameObject.FindGameObjectWithTag("Gold");
            Color temp = gold.GetComponent<Image>().color;
            temp.a = 1f;
            gold.GetComponent<Image>().color = temp;

            silver = GameObject.FindGameObjectWithTag("Silver");
            temp = silver.GetComponent<Image>().color;
            temp.a = .33f;
            silver.GetComponent<Image>().color = temp;

            bronze = GameObject.FindGameObjectWithTag("Bronze");
            temp = bronze.GetComponent<Image>().color;
            temp.a = .33f;
            bronze.GetComponent<Image>().color = temp;
        }
        catch {
            Debug.Log("Error: No UI element deteted");
        }

        //Find levelId
        for (int i = 0; i < OnLoad.Levels.Length; i++) {
            if (OnLoad.Levels[i].name == SceneManager.GetActiveScene().name) {
                levelId = i;
            }
        }

        OnLoad.Levels[levelId].targetSteps = targetSteps;

        //Get width x height
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("Tile");
        int val;
        foreach (GameObject block in blocks) {
            val = (int) block.transform.position.x;
            if (val >= width) {
                width = val + 1;
            }
            val = (int)block.transform.position.z;
            if (val >= height) {
                height = val + 1;
            }
        }
        val = goal[0];
        if (val > width) {
            width = val + 1;
        }
        val = goal[1];
        if (val > height) {
            height = val + 1;
        }


        grid = new List<EntityType>[width, height];

        tiles = new GameObject[width, height];

        Debug.Log(width + " / " + height);

        foreach (GameObject obj in blocks) {
            //Go through tiles and add a true
            //Afterwards, add a pit object to the grid for each null value
            //Debug.Log("Checking tile at: " + (int)obj.transform.position.x + " / " + (int)obj.transform.position.z);
            tiles[(int) Math.Round(obj.transform.position.x), (int) Math.Round(obj.transform.position.z)] = obj;
        }

        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                if (tiles[x, y] == null && !(x == goal[0] && y == goal[1])) {
                    //Spawn thing
                    GameObject model = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Pit pit = (Pit)model.AddComponent(System.Type.GetType("Pit"));
                    pit.Model = model;
                    spawner.Spawn(pit, x, y, -30f, "up", 0.01f, grid);
                }
            }
        }

        Dispose();
    }

    void Update() {
        //Check for inputs
        turnTimer += Time.deltaTime;

        /*
        //Pause for transition - set to 0f to disable
        if (turnTimer >= 0f && pause) {
            pause = false;
            turnTimer = 1.33f;
            try {
                GameObject transition = GameObject.FindGameObjectWithTag("Transition");
                transition.SetActive(false);
            }
            catch {
                Debug.Log("Error: No transition object found");
            }
        }
        */

        //Delay for extended explosion animation
        if (turnTimer >= 1.5f && explodePause) {
            explodePause = false;
            //turnTimer = 3f;
        }

        if (turnTimer >= 0.3f && !pause && !explodePause) {
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) { // W
                ProcessTurn("up");
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
            { // A
                ProcessTurn("left");
            }
            if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            { // S
                ProcessTurn("down");
            }
            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            { // D
                ProcessTurn("right");
            }
            if (Input.GetKeyUp(KeyCode.R)) { // R
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            if (Input.GetKeyUp(KeyCode.Q)) { //Q
                Dispose();
                string output = "";
                foreach (EntityType obj in movableList) {
                    output += obj.name + ", ";
                }
                Debug.Log(output);
            }
        }

        if (!winState && loseState) {
            Debug.Log("You lose :(");
            failScreen.SetActive(true);
            loseState = true;
            pause = true;
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
        Dispose();
        foreach (MovableObject obj in movableList) {
            obj.GetDestination(pDirection, false);
        }
        //Adjust destination based on collision
        foreach (MovableObject obj in movableList) {
            foreach (MovableObject comp in movableList) {
                if (obj != comp) {
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
        }
        Dispose(); //Why does this call stop the game from crashing???
        //Check if player's move was valid
        if (!(avatar.newX == avatar.X && avatar.newY == avatar.Y)) {
            //Update step count
            steps += 1;
            Color temp;
            //Display correct medal
            if (steps <= targetSteps) {
                //gold
                temp = gold.GetComponent<Image>().color;
                temp.a = 1f;
                gold.GetComponent<Image>().color = temp;

                temp = silver.GetComponent<Image>().color;
                temp.a = .33f;
                silver.GetComponent<Image>().color = temp;

                temp = bronze.GetComponent<Image>().color;
                temp.a = .33f;
                bronze.GetComponent<Image>().color = temp;
            }
            else if (steps <= targetSteps + 3) {
                //Silver
                temp = gold.GetComponent<Image>().color;
                temp.a = .33f;
                gold.GetComponent<Image>().color = temp;

                temp = silver.GetComponent<Image>().color;
                temp.a = 1f;
                silver.GetComponent<Image>().color = temp;

                temp = bronze.GetComponent<Image>().color;
                temp.a = .33f;
                bronze.GetComponent<Image>().color = temp;
            }
            else {
                //bronze
                temp = gold.GetComponent<Image>().color;
                temp.a = .33f;
                gold.GetComponent<Image>().color = temp;

                temp = silver.GetComponent<Image>().color;
                temp.a = .33f;
                silver.GetComponent<Image>().color = temp;

                temp = bronze.GetComponent<Image>().color;
                temp.a = 1f;
                bronze.GetComponent<Image>().color = temp;
            }
            try {
                GameObject display = GameObject.FindGameObjectWithTag("UI (Steps)");
                display.GetComponent<Text>().text = steps.ToString();
                if (steps > targetSteps) {
                    display.GetComponent<Text>().color = Color.red;
                }
                else {
                    display.GetComponent<Text>().color = Color.white;
                }
            }
            catch {
                Debug.Log("Error: No UI element deteted");
            }

            //Explode objects that collide with level edge
            foreach (MovableObject obj in movableList) {
                if (!obj.GetDestination(pDirection, true)) {
                    obj.audioSource.PlayOneShot(explosionHitWallSE);
                    obj.OnExplode(null);
                }
            }
            Dispose();
            //Adjust destination based on collision
            foreach (MovableObject obj in movableList) {
                foreach (MovableObject comp in movableList) {
                    if (obj != comp) {
                        PushBlock pushCheck1 = obj as PushBlock;
                        PushBlock pushCheck2 = comp as PushBlock;
                        if (obj.newY == comp.newY && obj.newX == comp.newX) {
                            if (obj.collision && comp.collision) {
                                obj.newX = obj.X;
                                obj.newY = obj.Y;
                                comp.newX = comp.X;
                                comp.newY = comp.Y;
                            }
                            //Check if any objct involved are pushable
                            else if (pushCheck1 != null || pushCheck2 != null) {                             
                                bool impact = false;
                                //Handle for push blocks
                                if (pushCheck1 != null) {   
                                    if (!obj.moveChecked) {
                                        Debug.Log(obj.name + " is a pushblock");
                                        switch (CalcDirection(comp.X, comp.Y, comp.newX, comp.newY)) {
                                            case "up":
                                                impact = CheckMove(obj.newX, obj.newY + 1);
                                                break;
                                            case "down":
                                                impact = CheckMove(obj.newX, obj.newY - 1);
                                                break;
                                            case "left":
                                                impact = CheckMove(obj.newX - 1, obj.newY);
                                                Debug.Log(obj.name + " was pushed from the left :)");
                                                break;
                                            case "right":
                                                impact = CheckMove(obj.newX + 1, obj.newY);
                                                break;
                                            default:
                                                Debug.Log(obj.name + " isn't being hit from any direct :C");
                                                break;
                                        }
                                        if (impact) {
                                            Debug.Log(obj.name + " was impacted by " + comp.name);
                                            obj.OnExplode(CalcDirection(comp.X, comp.Y, comp.newX, comp.newY));
                                        }
                                        else {
                                            Debug.Log(obj.name + " is blocked from moving :(");
                                            obj.audioSource.PlayOneShot(chairStuckSE, 1f);
                                            comp.OnExplode(CalcDirection(obj.X, obj.Y, obj.newX, obj.newY));
                                        }
                                    }
                                }
                                else if (!comp.moveChecked) {
                                    Debug.Log(comp.name + " is a pushblock");
                                    impact = false;
                                    switch (CalcDirection(obj.X, obj.Y, obj.newX, obj.newY)) {
                                        case "up":
                                            impact = !CheckMove(comp.newX, comp.newY + 1);
                                            break;
                                        case "down":
                                            impact = !CheckMove(comp.newX, comp.newY - 1);
                                            break;
                                        case "left":
                                            impact = !CheckMove(comp.newX - 1, comp.newY);
                                            break;
                                        case "right":
                                            impact = !CheckMove(comp.newX + 1, comp.newY);
                                            break;
                                        default:
                                            break;
                                    }
                                    if (impact) {
                                        comp.OnExplode(CalcDirection(obj.X, obj.Y, obj.newX, obj.newY));
                                    }
                                    else {
                                        Debug.Log(comp.name + " is blocked from moving :(");
                                        comp.audioSource.PlayOneShot(chairStuckSE, 1f);
                                        obj.OnExplode(CalcDirection(comp.X, comp.Y, comp.newX, comp.newY));
                                    }
                                }
                                obj.moveChecked = true;
                                comp.moveChecked = true;
                            }
                            //Handle 2 pulses colliding
                            else if (pushCheck1 == null && pushCheck2 == null) {
                                obj.audioSource.PlayOneShot(explosionPulseCollisionSE);
                                Debug.Log("Trigger collision explosion" + obj.name + " and " + comp.name);
                                if (!obj.moveChecked) {
                                    obj.OnExplode(CalcDirection(comp.X, comp.Y, comp.newX, comp.newY));
                                }
                                if (!comp.moveChecked) {
                                    comp.OnExplode(CalcDirection(obj.X, obj.Y, obj.newX, obj.newY));
                                }
                                obj.moveChecked = true;
                                comp.moveChecked = true;
                            }
                        }
                    }
                }
            }
            Dispose();
            foreach (MovableObject obj in movableList) {
                obj.moveChecked = false;
            }
            Dispose();
            //Move all objects to new coords
            foreach (MovableObject obj in movableList) {
                obj.Move();
            }
            Dispose();
            //Check win state
            if (avatar.X == goal[0] && avatar.Y == goal[1]) {
                winState = true;
                Debug.Log("You win!!!");
                //Update cleared status
                OnLoad.Levels[levelId].cleared = true;
                //Save step count
                if (OnLoad.Levels[levelId].steps > steps) {
                    OnLoad.Levels[levelId].steps = steps;
                }
                //Save progress
                if (levelId == OnLoad.Levels.Length - 1) {
                    SaveGame(levelId);
                }
                else {
                    SaveGame(levelId + 1);
                }
                //Dislay finish screen
                finishScreen.SetActive(true);
                finishButton.SetActive(true);
                walrusIcon.GetComponent<Animator>().Play("WIN"); ;
            }
            turnTimer = 0f;
        }
        else {
            avatar.audioSource.PlayOneShot(characterHitWallSE);
            avatar.Rotate(pDirection);
        }
        Dispose();
    }

    private void Dispose() {
        IRemovable temp;
        for (int i = 0; i < movableList.Count; i++) {
            temp = movableList[i] as IRemovable;
            if (temp != null) {
                if (temp.Terminate) {
                    movableList.Remove(movableList[i]);
                }
            }
        }
        foreach (List<EntityType> tile in grid) {
            if (tile != null) {
                for (int i = 0; i < tile.Count; i++) {
                    temp = tile[i] as IRemovable;
                    if (temp != null) {
                        if (temp.Terminate) {
                            tile.Remove(tile[i]);
                        }
                    }
                }
            }
        }
    }

    private void SaveGame (int progress) {
        OnLoad.Progress = progress;
        Save save = new Save(OnLoad.Progress, OnLoad.Levels);
        
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
    }

    private string CalcDirection (int x, int y, int nX, int nY) {
        if (nY < y) {
            return "down";
        }
        else if (nY > y) {
            return "up";
        }
        else if (nX < x) {
            return "left";
        }
        else if (nX > x) {
            return "right";
        }
        return null;
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

    private void GenerateLevel() {
        if (GameObject.FindWithTag("End") != null) {
            goal = new int[2] { (int)GameObject.FindWithTag("End").transform.position.x, (int)GameObject.FindWithTag("End").transform.position.z };
        }
        else {
            goal = new int[2] { 0, 0 };
            Debug.Log("Please add an 'End' tile to this level");
        }
        Debug.Log("Gen level");
        grid = new List<EntityType>[width, height];
        //avModel = GameObject.FindWithTag("Avatar");
        //avModel.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Controllers/player_controller") as RuntimeAnimatorController;

        //Temp - remove after IdleChan is repalced
        //avModel.GetComponent<UnityChan.IdleChanger>().enabled = false;
        //avModel.GetComponent<UnityChan.FaceUpdate>().enabled = false;

        //avModel = Instantiate(avModel);
        //avatar = new Avatar(9, 9, -0.5f, avModel, new Pulsar(), 3, tiles);
        //avatar = new Avatar(9, 9, GameObject.FindWithTag("Player"), new Pulsar(), 3, tiles);
        avatar = GameObject.FindWithTag("Avatar").GetComponent<Avatar>();
        //List<EntityType> obstacles = new List<EntityType>();
        //Material m1 = Resources.Load("Friends", typeof(Material)) as Material;
        //Material m2 = null;
        
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
            nBlock = spawner.SpawnTile(block, xCount, yCount, -0.5f, grid);

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

    public void LoseGame() {
        if (!winState) {
            loseState = true;
        }
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}