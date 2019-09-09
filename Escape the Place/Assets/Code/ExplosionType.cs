using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public abstract class ExplosionType
{

    public abstract bool Explode(Avatar avatar, List<EntityType>[,] grid);

}