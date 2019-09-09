using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakBlock : EntityType, IRemovable {
    float deathTimer = 1.333f;
    public bool terminate;

    public BreakBlock() : base(true) {

    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        Debug.Log(name + " broke!");
        terminate = true;
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
