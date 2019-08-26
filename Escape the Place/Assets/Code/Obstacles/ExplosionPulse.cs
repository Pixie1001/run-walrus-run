using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class ExplosionPulse : MovableObject {
    public string direction;

    public ExplosionPulse() : base(false) {
        //model = Resources.Load("Prefabs/Polygonal Metalon Red") as GameObject;
        //model.AddComponent(Type.GetType("ExplosionPulse"));
        //Debug.Log("New EP script created");
    }

    public new GameObject Model {
        get {
            return model;
        }
        set {
            model = value;
        }
    }
}