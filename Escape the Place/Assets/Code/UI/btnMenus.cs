using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class btnMenus : MonoBehaviour
{
    public GameObject legend;

    FileStream file;

    protected void Start() {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            try {
                Debug.Log(Application.persistentDataPath + "/gamesave.save");
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save data = (Save)bf.Deserialize(file);
                file.Close();

                OnLoad.Progress = data.progress;
            }
            catch {
                Debug.Log("Error: Save data corrupted!");
                if (file != null) {
                    file.Close();
                }
                File.Delete(Application.persistentDataPath + "/gamesave.save");
                UnityEditor.AssetDatabase.Refresh();
                //delete folder somehow
            }
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
        SceneManager.LoadScene(OnLoad.Levels[OnLoad.Progress].name);
    }

    public void Return() {
        SceneManager.LoadScene("Title Page");
    }
}
