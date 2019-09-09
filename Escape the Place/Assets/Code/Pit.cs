using UnityEngine;
using System.Collections;

public class Pit : EntityType
{
    public Pit () : base(true) {
        //this.x = x;
        //this.y = x;
    }

    public new GameObject Model
    {
        get {
            return model;
        }
        set {
            model = value;
        }
    }
}