using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnMenus : ButtonType
{
    public GameObject legend;
    public GameObject popup;

    public void Rules() {
        if (!legend.activeSelf) {
            PlaySE();
            legend.SetActive(true);
        }
        else {
            GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
            legend.SetActive(false);
        }
    }

    public void StartGame() {
        PlaySE();
        popup.SetActive(true);
        popup.transform.GetChild(0).gameObject.SetActive(true);
        popup.transform.GetChild(1).gameObject.SetActive(false);
        popup.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void Continue() {
        PlaySE();
        SceneManager.LoadScene(OnLoad.Levels[OnLoad.Progress].name);
    }

    public void Return() {
        PlaySE();
        SceneManager.LoadScene("Title Page");
    }

    public void Exit() {
        PlaySE();
        Debug.Log("Quitting application");
        Application.Quit();
    }
}
