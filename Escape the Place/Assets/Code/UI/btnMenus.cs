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
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save data = (Save)bf.Deserialize(file);
            file.Close();

            OnLoad.Progress = data.progress;
        }
        else {
            Debug.Log("No save data found");
        }
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
        SceneManager.LoadScene(OnLoad.Levels[OnLoad.Progress]);
    }

    public void Return() {
        SceneManager.LoadScene("Title Page");
    }
}
