using UnityEngine;
using System.Collections;

public class DustPulse : MonoBehaviour
{
    float targetTime = 1.1f;
    float currTime = 0f;
    // Use this for initialization
    void Start() {
        Debug.Log("Cloud is spawned");
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
