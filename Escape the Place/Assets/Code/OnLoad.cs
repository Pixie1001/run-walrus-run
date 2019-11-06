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
    public static float sfx = 1, bgm = 1;
    private static LevelData[] levels = { new LevelData("Puzzle 1"), new LevelData("Puzzle 2"), new LevelData("Puzzle 3"), new LevelData("Puzzle 4"), new LevelData("Puzzle 5"), new LevelData("Puzzle 6") };

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

    public static LevelData[] Levels
    {
        get {
            return levels;
        }
        set {
            levels = value;
        }
    }
}
