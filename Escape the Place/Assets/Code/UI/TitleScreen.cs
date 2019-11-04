using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TitleScreen : MonoBehaviour
{
    FileStream file;

    // Use this for initialization
    void Start() {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            try {
                Debug.Log(Application.persistentDataPath + "/gamesave.save");
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save data = (Save)bf.Deserialize(file);
                file.Close();

                OnLoad.Progress = data.progress;
                OnLoad.Levels = data.levels;
            }
            catch {
                Debug.Log("Error: Save data corrupted!");
                if (file != null) {
                    file.Close();
                }
                File.Delete(Application.persistentDataPath + "/gamesave.save");
                //delete folder somehow
            }
        }
        else {
            Debug.Log("No save data found");
        }

    }

    // Update is called once per frame
    void Update() {

    }
}
