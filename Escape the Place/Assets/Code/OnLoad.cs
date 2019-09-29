using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[InitializeOnLoad]
public static class OnLoad {
    /*
    //public const float speed = 1f;

    //public enum ExplodeResult { nothing, lose, push };

	// Use this for initialization
	static OnLoad () {
        Debug.Log("On load script :D");
	}
    */

    private static int progress;

    private static string[] levels = { "Puzzle 1", "Puzzle 3", "Puzzle 4", "Puzzle 5" };

    public static int Progress
    {
        get {
            return progress;
        }
        set {
            if (value >= progress) {
                progress = value;
            }
        }
    }

    public static string[] Levels
    {
        get {
            return levels;
        }
    }
}
