using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData
{
    public string name;
    public int steps = 0;
    public int targetSteps;

    public LevelData(string levelName) {
        name = levelName;
    }
}
