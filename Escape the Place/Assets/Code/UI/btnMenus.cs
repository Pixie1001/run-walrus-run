using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnMenus : MonoBehaviour
{
    public GameObject legend;
    public GameObject popup;

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
        popup.SetActive(true);
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
