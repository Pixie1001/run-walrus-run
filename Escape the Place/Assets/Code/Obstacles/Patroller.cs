using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Patroller : MovableObject
{
    public string[] route;
    private int step;

    public Patroller (GameObject _model, int _x, int _y, float _elevation, string[] _route, List<ObstacleType>[,] _grid) : base (true, _model, _x, _y, _elevation, _grid) {
        route = _route;
        step = 0;
    }

    public bool Move(List<ObstacleType>[,] grid) {
        bool result = Move(route[step], grid);
        if (result) {
            step += 1;
            if (step > route.Length) {
                step = 0;
            }
        }
        return result;
    }


}