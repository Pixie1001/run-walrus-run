using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MovableObject {

    public enum ExplosionTypes { Pulsar };
    public int explodeOn;
    ExplosionType explosionType;
    public ExplosionTypes PickExplosion;

    public int turnCount = 0;

    public Avatar () : base (true) {
        if (explosionType == null) {
            explosionType = new Pulsar();
        }

        if (explodeOn == null) {
            explodeOn = 3;
        }

        switch (PickExplosion) {
            case ExplosionTypes.Pulsar:
                explosionType = new Pulsar();
                break;
            default:
                explosionType = new Pulsar();
                break;
        }
    }

    public void PlayerMove(string direction, List<EntityType>[,] grid) {
        if (Move(direction, grid)) {
            turnCount += 1;
            if (turnCount >= explodeOn) {
                explosionType.Explode(this, grid);
                turnCount = 0;
            }
        }
    }
}
