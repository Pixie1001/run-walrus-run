using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class btnOptions : ButtonType
{
    public GameObject options;

    public void Options() {
        PlaySE();
        options.SetActive(true);
        Debug.Log("OnLOad.sfx = " + OnLoad.sfx);
        options.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = OnLoad.sfx;
        options.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Slider>().value = OnLoad.bgm;
    }

    public void Return() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();

        Save save = new Save(OnLoad.Progress, OnLoad.Levels, OnLoad.sfx, OnLoad.bgm);

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log(save.sfx);

        options.SetActive(false);
    }
}
