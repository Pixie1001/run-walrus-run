using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnMove : ButtonType
{
    TileGrid main;

    // Use this for initialization
    override protected void Start() {
        base.Start();
        main = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TileGrid>();
    }

    public void MoveUp() {
        PlaySE();
        if (!main.pause) {
            main.ProcessTurn("up");
        }
    }

    public void MoveLeft() {
        PlaySE();
        if (!main.pause) {
            main.ProcessTurn("left");
        }
    }

    public void MoveRight() {
        PlaySE();
        if (!main.pause) {
            main.ProcessTurn("right");
        }
    }

    public void MoveDown() {
        PlaySE();
        if (!main.pause) {
            main.ProcessTurn("down");
        }
    }
}
