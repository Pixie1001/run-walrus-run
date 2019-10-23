using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : MovableObject {


    public enum ExplosionTypes { Pulsar };
    public int explodeOn;
    float expClock = 0f;
    float expTime = 0.7f;
    ExplosionType explosionType;
    public ExplosionTypes PickExplosion;
    bool triggerExplosion1 = false;
    bool triggerExplosion2 = false;
    AudioClip moveSE, explodeSE, slipSE, walkIntoObjectSE;


    int countdown;

    public Avatar () : base (true) {
        if (explosionType == null) {
            explosionType = new Pulsar();
        }

        if (explodeOn == 0) {
            explodeOn = 3;
        }

        switch (PickExplosion) {
            case ExplosionTypes.Pulsar:
                explosionType = new Pulsar();
                break;
            default:
                explosionType = new Pulsar();
                break;
        }
    }

    protected override void Start() {
        base.Start();
        collision = true;

        //Sound stuff
        moveSE = Resources.Load<AudioClip>("Audio/Upload/CharacterMove");
        //audioSource.clip = moveSE;
        //audioSource.volume = 10f;
        explodeSE = Resources.Load<AudioClip>("Audio/Upload/CharacterExplosion");
        slipSE = Resources.Load<AudioClip>("Audio/Upload/CharacterSlip");

        explodeOn = explodeOn - 1;
        countdown = explodeOn;

        try {
            GameObject display = GameObject.FindGameObjectWithTag("UI (Countdown)");
            display.GetComponent<Text>().text = (explodeOn).ToString();
            //Adjust colour to signal imminent explosion
            if (countdown == 1) {
                display.GetComponent<Text>().color = Color.red;
                GameObject.FindGameObjectWithTag("FallIcon").GetComponent<Animator>().Play("EXPLODE");
            }
            else {
                display.GetComponent<Text>().color = Color.white;
            }
        }
        catch {
            Debug.Log("Error: No UI element detected");
        }
    }

    private T AddComponent<T>() {
        throw new NotImplementedException();
    }

    protected override void Update() {
        base.Update();
        if (currTime >= targetTime - 0.5f && triggerExplosion1) {
            audioSource.PlayOneShot(slipSE);
            GetComponent<Animator>().Play("EXPLODE");
            triggerExplosion1 = false;
            triggerExplosion2 = true;
        }

        if (expClock >= expTime && triggerExplosion2) {
            audioSource.PlayOneShot(explodeSE);
            explosionType.Explode(this, grid);
            DustCloud();
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().Play("EXPLODE");
            triggerExplosion2 = false;
        }
        else {
            expClock += Time.deltaTime;
        }
    }

    public override void Move() {
        //audioSource.PlayOneShot(moveSE);
        audioSource.PlayOneShot(moveSE, 5f);
        base.Move();
        countdown -= 1;
        if (countdown == 0) {
            triggerExplosion1 = true;
            expClock = 0;
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TileGrid>().explodePause = true;
        }
        else if (countdown < 0) {
            countdown = explodeOn;
        }

        //UpdateUI
        try {
            GameObject display = GameObject.FindGameObjectWithTag("UI (Countdown)");
            string countDisplay;
            if (countdown == 0) {
                countDisplay = (explodeOn + 1).ToString();
            }
            else {
                countDisplay = countdown.ToString();
            }
            display.GetComponent<Text>().text = countDisplay;
            //Adjust colour to signal imminent explosion
            if (countdown == 1) {
                display.GetComponent<Text>().color = Color.red;
                GameObject.FindGameObjectWithTag("FallIcon").GetComponent<Animator>().Play("EXPLODE");
            }
            else {
                display.GetComponent<Text>().color = Color.white;
            }
        }
        catch {
            Debug.Log("Error: No UI element deteted");
        }
    }

    public void Rotate(string direction) {
        float rotate;
        switch (direction) {
            case "up":
                rotate = 0f;
                break;
            case "down":
                rotate = 180f;
                break;
            case "left":
                rotate = rotate = -90f;
                break;
            case "right":
                rotate = 90f;
                break;
            default:
                Debug.Log("Invalid turn?");
                rotate = 0f;
                break;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotate, transform.eulerAngles.z);
        audioSource.PlayOneShot(walkIntoObjectSE);
    }

    private void DustCloud() {
        Debug.Log("Spawn cloud");
        //Generate dust cloud
        GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/DustPulse") as GameObject);
        DustPulse script = (DustPulse)obj.AddComponent(System.Type.GetType("DustPulse"));
        //script.enabled = true;
        obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        new ObjectSpawner().SpawnTile(obj, X, Y, 0.02f, grid);
    }
}





/*
public bool PlayerMove(string direction) {
    if (Move(direction)) {
        turnCount += 1;
        if (turnCount >= explodeOn) {
            explosionType.Explode(this, grid);
            turnCount = 0;
        }
        return true;
    }
    return false;
}
*/
