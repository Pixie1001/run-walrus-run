using UnityEngine;
using UnityEditor;

public class Pulsar : IExplosionType
{

    public bool Explode() {
        Debug.Log("EXPLOSION!!!");
        return true;
    }

}