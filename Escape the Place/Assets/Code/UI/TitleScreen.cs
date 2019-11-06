using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TitleScreen : MonoBehaviour
{
    FileStream file;
    AudioSource audio;

    // Use this for initialization
    void Start() {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save")) {
            try {
                Debug.Log(Application.persistentDataPath + "/gamesave.save");
                BinaryFormatter bf = new BinaryFormatter();
                file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                Save data = (Save)bf.Deserialize(file);
                file.Close();

                Debug.Log("sfx = " + data.sfx);

                OnLoad.Progress = data.progress;
                OnLoad.Levels = data.levels;
                OnLoad.sfx = data.sfx;
                OnLoad.bgm = data.bgm;
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

        audio = (AudioSource)gameObject.AddComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Audio/BGM");
        audio.loop = true;
        audio.Play();
        audio.volume = 0.25f * OnLoad.bgm;

        Debug.Log("OnLOad.sfx = " + OnLoad.sfx);
    }

    public void UpdateAudio() {
        audio.volume = 0.25f * OnLoad.bgm;
    }
}
