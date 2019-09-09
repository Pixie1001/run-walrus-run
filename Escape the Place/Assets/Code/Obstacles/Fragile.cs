using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fragile : EntityType, IRemovable
{
    float deathTimer = 1.333f;
    public bool terminate;

    public Fragile () : base(true) {

    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        terminate = true;
        GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().loseState = true;
        if (Model.GetComponent<Animator>() != null) {
            Model.GetComponent<Animator>().Play("EXPLODE");
        }
    }

    protected override void Update() {
        base.Update();
        if (terminate) {
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer <= 0) {
            GameObject.Destroy(Model);
        }
    }

    public bool Terminate
    {
        get {
            return terminate;
        }
    }
}
