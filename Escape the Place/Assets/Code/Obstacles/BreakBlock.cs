﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakBlock : EntityType, IRemovable {
    float deathTimer = 1.333f;
    public bool terminate;
    AudioClip explodeSE;

    public BreakBlock() : base(true) {

    }

    protected override void Start() {
        base.Start();
        explodeSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionBreakBlock");
    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        Debug.Log(name + " broke!");
        terminate = true;
        audioSource.PlayOneShot(explodeSE, OnLoad.sfx);
        if (Model.GetComponent<Animator>() != null) {
            Model.GetComponent<Animator>().Play("EXPLODE");
        }
        else {
            deathTimer = 0f;
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
