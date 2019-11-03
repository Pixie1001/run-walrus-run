using UnityEngine;
using System.Collections;

public class ExitArrow : MonoBehaviour
{
    float targetTime = 4f;
    float currTime = 0f;
    // Use this for initialization

    // Update is called once per frame
    protected void Update() {
        if (currTime <= targetTime) {
            currTime += Time.deltaTime;
        }
        else {
            Destroy(gameObject);
        }
    }
}
