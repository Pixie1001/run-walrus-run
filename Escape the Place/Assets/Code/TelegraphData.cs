using UnityEngine;
using System.Collections;

public class TelegraphData
{
    public GameObject model;
    public int x, y;
    public Color color;

    public TelegraphData(GameObject _model, int _x, int _y, Color _color) {
        model = _model;
        x = _x;
        y = _y;
        color = _color;
    }
}
