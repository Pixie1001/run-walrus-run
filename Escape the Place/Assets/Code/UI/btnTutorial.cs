using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnTutorial : MonoBehaviour
{
    public void Close() {
        SceneManager.LoadScene("Puzzle 1");
    }
}