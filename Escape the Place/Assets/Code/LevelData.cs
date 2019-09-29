using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData
{
    public string name;
    public int steps = 0;

    public LevelData(string levelName) {
        name = levelName;
    }
}
