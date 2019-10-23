using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnFinish : MonoBehaviour
{
    int levelId;

    // Use this for initialization
    void Start() {
        //Find levelId
        for (int i = 0; i < OnLoad.Levels.Length; i++) {
            if (OnLoad.Levels[i].name == SceneManager.GetActiveScene().name) {
                levelId = i;
            }
        }
    }

    public void Finish() {

    }
}
