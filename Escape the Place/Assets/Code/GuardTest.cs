﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTest : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("1")) {
            anim.Play("RUN00_F");
        }
		
	}
}
