using UnityEngine;
using System.Collections;

public class TelegraphTile : EntityType
{
    float targetTime = 1f;
    float currTime = 0f;
    // Use this for initialization

    public TelegraphTile() : base (false) {

    }

    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        /*
        if (currTime <= targetTime) {
            currTime += Time.deltaTime;
        }
        else {
            Destroy(gameObject);
        }
        */
    }
}
