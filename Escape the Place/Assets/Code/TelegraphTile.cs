using UnityEngine;
using System.Collections;

public class TelegraphTile : MonoBehaviour
{
    float targetTime = 1f;
    float currTime = 0f;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (currTime <= targetTime) {
            currTime += Time.deltaTime;
        }
        else {
            Destroy(gameObject);
        }
    }
}
