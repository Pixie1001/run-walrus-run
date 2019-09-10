using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MovableObject {

    public enum ExplosionTypes { Pulsar };
    public int explodeOn;
    ExplosionType explosionType;
    public ExplosionTypes PickExplosion;


    int countdown = 0;

    public Avatar () : base (true) {
        if (explosionType == null) {
            explosionType = new Pulsar();
        }

        if (explodeOn == null) {
            explodeOn = 3;
        }

        countdown = explodeOn;

        switch (PickExplosion) {
            case ExplosionTypes.Pulsar:
                explosionType = new Pulsar();
                break;
            default:
                explosionType = new Pulsar();
                break;
        }
        if (this.collision) {
            Debug.Log("Construct (Avatar) = collision");
        }
        else {
            Debug.Log("Construct (Avatar) = clip");
        }
    }

    protected override void Start() {
        base.Start();
        collision = true;
        try {
            GameObject display = GameObject.FindGameObjectWithTag("UI (Countdown)");
            display.GetComponent<Text>().text = (explodeOn).ToString();
        }
        catch {
            Debug.Log("Error: No UI element deteted");
        }
    }

    public override void Move() {
        base.Move();
        countdown -= 1;
        if (countdown == 0) {
            explosionType.Explode(this, grid);
        }
        else if (countdown < 0) {
            countdown = explodeOn - 1;
        }

        //UpdateUI
        try {
            GameObject display = GameObject.FindGameObjectWithTag("UI (Countdown)");
            string countDisplay;
            if (countdown == 0) {
                countDisplay = (explodeOn).ToString();
            }
            else {
                countDisplay = countdown.ToString();
            }
            display.GetComponent<Text>().text = countDisplay;
        }
        catch {
            Debug.Log("Error: No UI element deteted");
        }
    }

    public void Rotate(string direction) {
        float rotate;
        switch (direction) {
            case "up":
                rotate = 0f;
                break;
            case "down":
                rotate = 180f;
                break;
            case "left":
                rotate = rotate = -90f;
                break;
            case "right":
                rotate = 90f;
                break;
            default:
                Debug.Log("Invalid turn?");
                rotate = 0f;
                break;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotate, transform.eulerAngles.z);
    }
}





/*
public bool PlayerMove(string direction) {
    if (Move(direction)) {
        turnCount += 1;
        if (turnCount >= explodeOn) {
            explosionType.Explode(this, grid);
            turnCount = 0;
        }
        return true;
    }
    return false;
}
*/
