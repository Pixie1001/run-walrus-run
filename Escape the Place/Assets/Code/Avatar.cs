using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MovableObject {

    float speed = OnLoad.speed;
    int explode;
    IExplosionType explosionType;
    public int x;
    public int y;

    int turnCount = 0;

    public Avatar (int x, int y, float elevation, GameObject model, IExplosionType explosionType, int explodeCount, List<ObstacleType>[,] grid) : base (true, model, x, y, elevation, grid) {
        this.x = x;
        this.y = y;
        this.explosionType = explosionType;
        this.explode = explodeCount;
    }
	
    public void PlayerMove(string direction, List<ObstacleType>[,] grid) {
        if (Move(direction, grid)) {
            turnCount += 1;
            if (turnCount >= explode) {
                explosionType.Explode();
                turnCount = 0;
            }
        }
    }
}
