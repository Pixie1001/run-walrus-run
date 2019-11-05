using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fragile : EntityType, IRemovable
{
    float deathTimer = 1.333f;
    public bool terminate;
    AudioClip explodeSE;

    public Fragile () : base(true) {

    }

    protected override void Start() {
        base.Start();
        explodeSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitFailState");
    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        terminate = true;
        GameObject.FindWithTag("MainCamera").GetComponent<TileGrid>().LoseGame();
        audioSource.PlayOneShot(explodeSE, .75f);
        if (Model.GetComponent<Animator>() != null) {
            Model.GetComponent<Animator>().Play("EXPLODE");
            Model.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    protected override void Update() {
        base.Update();
        /*
        if (terminate) {
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer <= 0) {
            GameObject.Destroy(Model);
        }
        */
    }

    public bool Terminate
    {
        get {
            return terminate;
        }
    }
}
