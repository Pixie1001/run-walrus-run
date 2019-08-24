using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class OnLoad {

    public const float speed = 1f;

	// Use this for initialization
	static OnLoad () {
        Debug.Log("On load script :D");
	}
}
