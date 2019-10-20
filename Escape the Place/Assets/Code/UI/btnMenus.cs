using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnMenus : MonoBehaviour
{
    public GameObject legend;

    protected void Start() {
        
    }

    public void Rules() {
        if (!legend.activeSelf) {
            legend.SetActive(true);
        }
        else {
            legend.SetActive(false);
        }
    }

    public void StartGame() {
        SceneManager.LoadScene("Puzzle 1");
    }

    public void Continue() {
        SceneManager.LoadScene(OnLoad.Levels[OnLoad.Progress].name);
    }

    public void Return() {
        SceneManager.LoadScene("Title Page");
    }

    public void Exit() {
        Debug.Log("Quitting application");
        Application.Quit();
    }
}
