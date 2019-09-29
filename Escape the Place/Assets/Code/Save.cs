using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int progress;
    public LevelData[] levels;

    public Save (int nProgress, LevelData[] nLevels) {
        progress = nProgress;
        levels = nLevels;
    }
}
