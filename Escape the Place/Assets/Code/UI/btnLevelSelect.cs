using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class btnLevelSelect : ButtonType
{
    public GameObject selectScreen;
    float alpha = 0.2f;

    override protected void Start() {
        base.Start();
        Debug.Log("Progress: " + OnLoad.Progress);
    }

    public void SelectMenu() {
        if (!selectScreen.activeSelf) {
            PlaySE();
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
            if (OnLoad.Progress < 5) {
                GameObject.FindGameObjectWithTag("Lvl6").GetComponent<Image>().color = new Color(1, 1f, 1f, alpha);
                GameObject.FindGameObjectWithTag("Lvl6").GetComponentsInChildren<Text>()[0].color = new Color(1f, 1f, 1f, alpha);
            }
            DisplayMedal(GameObject.FindGameObjectWithTag("Lvl1").transform.GetChild(1).gameObject, 0);
            DisplayMedal(GameObject.FindGameObjectWithTag("Level 2").transform.GetChild(1).gameObject, 1);
            DisplayMedal(GameObject.FindGameObjectWithTag("Lvl3").transform.GetChild(1).gameObject, 2);
            DisplayMedal(GameObject.FindGameObjectWithTag("Lvl4").transform.GetChild(1).gameObject, 3);
            DisplayMedal(GameObject.FindGameObjectWithTag("Lvl5").transform.GetChild(1).gameObject, 4);
            DisplayMedal(GameObject.FindGameObjectWithTag("Lvl6").transform.GetChild(1).gameObject, 5);
        }
        else {
            GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
            selectScreen.SetActive(false);

        }
    }

    public void Level_1() {
        PlaySE();
        SceneManager.LoadScene(OnLoad.Levels[0].name);
    }

    public void Level_2() {
        PlaySE();
        if (OnLoad.Progress >= 1) {
            SceneManager.LoadScene(OnLoad.Levels[1].name);
        }
    }

    public void Level_3() {
        PlaySE();
        if (OnLoad.Progress >= 2) {
            SceneManager.LoadScene(OnLoad.Levels[2].name);
        }
    }

    public void Level_4() {
        PlaySE();
        if (OnLoad.Progress >= 3) {
            SceneManager.LoadScene(OnLoad.Levels[3].name);
        }
    }

    public void Level_5() {
        PlaySE();
        if (OnLoad.Progress >= 4) {
            SceneManager.LoadScene(OnLoad.Levels[4].name);
        }
    }

    private void DisplayMedal(GameObject medal, int levelNum) {
        //Display correct medal
        if (!OnLoad.Levels[levelNum].cleared) {
            //None
            medal.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
        }
        else if (OnLoad.Levels[levelNum].steps <= OnLoad.Levels[levelNum].targetSteps) {
            //gold
            Debug.Log("Gold: " + OnLoad.Levels[levelNum].steps + " <=  " + OnLoad.Levels[levelNum].targetSteps);
            medal.GetComponent<Image>().color = new Color(1f, .745f, 0.3647f);
        }
        else if (OnLoad.Levels[levelNum].steps <= OnLoad.Levels[levelNum].targetSteps + 3) {
            //Silver
            Debug.Log("Silver: " + OnLoad.Levels[levelNum].steps + " <=  " + OnLoad.Levels[levelNum].targetSteps + 3);
            medal.GetComponent<Image>().color = new Color(0.8117f, 0.8117f, 0.8117f);
        }
        else {
            //bronze
            Debug.Log("Bronze: " + OnLoad.Levels[levelNum].steps + " >  " + OnLoad.Levels[levelNum].targetSteps + 3);
            medal.GetComponent<Image>().color = new Color(0.7372f, 0.6078f, 0.5098f);
        }
    }

}