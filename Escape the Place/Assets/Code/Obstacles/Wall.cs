using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Wall : ObstacleType
{
    public Wall(int x, int y, float elevation, GameObject model, List<ObstacleType>[,] grid) : base(true, model, x, y, elevation, grid) {
        
    }
}