using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnTutorial : ButtonType
{
    public void Close() {
        PlaySE();
        SceneManager.LoadScene("Puzzle 1");
    }
}