﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Patroller : MovableObject
{
    public string[] route;
    private int step;

    public Patroller (string[] _route) : base (true) {
        route = _route;
        step = 0;
    }

    /*
    public bool Move() {
        bool result = Move(route[step]);
        if (result) {
            step += 1;
            if (step > route.Length) {
                step = 0;
            }
        }
        return result;
    }
    */


}