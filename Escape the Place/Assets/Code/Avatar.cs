using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void Move() {
        base.Move();
        countdown -= 1;
        if (countdown == 0) {
            explosionType.Explode(this, grid);
        }
        else if (countdown < 0) {
            countdown = explodeOn;
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
