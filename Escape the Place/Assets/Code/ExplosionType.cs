using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class ExplosionType : MonoBehaviour
{

    void Start() {
        Debug.Log("Worked");
    }

    public abstract bool Explode(Avatar avatar, List<EntityType>[,] grid);

}