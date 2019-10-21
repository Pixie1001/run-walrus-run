using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class btnLevelSelect : MonoBehaviour
{
    public GameObject selectScreen;
    float alpha = 0.2f;

    protected void Start() {

    }

    public void SelectMenu() {
        if (!selectScreen.activeSelf) {
            selectScreen.SetActive(true);
            //Change button colours based on prog
            if (OnLoad.Progress < 1) {
                GameObject.FindGameObjectWithTag("Level 2").GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Level 2").GetComponentsInChildren<Text>()[0].color = new Color(1f, 1f, 1f, alpha);
            }
            if (OnLoad.Progress < 2) {
                GameObject.FindGameObjectWithTag("Lvl3").GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl3").GetComponentsInChildren<Text>()[0].color = new Color(1f, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl4").GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl5").GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
            }
            if (OnLoad.Progress < 3) {
                GameObject.FindGameObjectWithTag("Lvl4").GetComponent<Image>().color = new Color(1f, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl4").GetComponentsInChildren<Text>()[0].color = new Color(1f, 1f, 1f, alpha);
            }
            if (OnLoad.Progress < 4) {
                GameObject.FindGameObjectWithTag("Lvl5").GetComponent<Image>().color = new Color(1, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl5").GetComponentsInChildren<Text>()[0].color = new Color(1f, 1f, 1f, alpha);
            }
        }
        else {
            selectScreen.SetActive(false);
        }
    }

    public void Level_1() {
        SceneManager.LoadScene(OnLoad.Levels[0].name);
    }

    public void Level_2() {
        if (OnLoad.Progress >= 1) {
            SceneManager.LoadScene(OnLoad.Levels[1].name);
        }
    }

    public void Level_3() {
        if (OnLoad.Progress >= 2) {
            SceneManager.LoadScene(OnLoad.Levels[2].name);
        }
    }

    public void Level_4() {
        if (OnLoad.Progress >= 3) {
            SceneManager.LoadScene(OnLoad.Levels[3].name);
        }
    }

    public void Level_5() {
        if (OnLoad.Progress >= 4) {
            SceneManager.LoadScene(OnLoad.Levels[4].name);
        }
    }
}