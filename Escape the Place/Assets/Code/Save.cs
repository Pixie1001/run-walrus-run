using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int progress;
    public LevelData[] levels;
    public float sfx, bgm;

    public Save (int nProgress, LevelData[] nLevels, float _sfx, float _bgm) {
        progress = nProgress;
        levels = nLevels;
        sfx = _sfx;
        bgm = _bgm;
    }
}
