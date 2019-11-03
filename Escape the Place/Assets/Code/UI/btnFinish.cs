using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnFinish : ButtonType
{
    int levelId;

    // Use this for initialization
    override protected void Start() {
        base.Start();
        //Find levelId
        for (int i = 0; i < OnLoad.Levels.Length; i++) {
            if (OnLoad.Levels[i].name == SceneManager.GetActiveScene().name) {
                levelId = i;
            }
        }
    }

    public void Finish() {
        PlaySE();
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
