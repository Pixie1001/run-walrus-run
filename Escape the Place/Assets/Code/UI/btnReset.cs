using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnReset : ButtonType
{

    public void Reset() {
        PlaySE();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
