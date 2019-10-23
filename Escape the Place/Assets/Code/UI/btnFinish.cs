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
        if (levelId == OnLoad.Levels.Length - 1) {
            SceneManager.LoadScene(OnLoad.Levels[0].name);
            Debug.Log("(Finished game) Load " + OnLoad.Levels[0]);
        }
        else {
            SceneManager.LoadScene(OnLoad.Levels[levelId + 1].name);
            Debug.Log("Load " + OnLoad.Levels[levelId + 1]);
        }
    }
}
