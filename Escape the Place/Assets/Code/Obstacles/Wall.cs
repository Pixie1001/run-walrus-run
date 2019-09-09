using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Wall : EntityType
{
    public Wall() : base(true) {
        
    }

    protected override void Start() {
        base.Start();
        //Set transparent
    }
}