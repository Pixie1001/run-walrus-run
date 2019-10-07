using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Wall : EntityType
{
    AudioClip explodeSE;

    public Wall() : base(true) {
        
    }

    protected override void Start() {
        base.Start();
        explodeSE = Resources.Load<AudioClip>("Audio/Upload/ExplosionHitWall");
        //Set transparent
    }

    public override void OnExplode(string dir) {
        base.OnExplode(dir);
        audioSource.PlayOneShot(explodeSE);
    }
}