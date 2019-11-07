using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class btnInfo: ButtonType
{
    public GameObject legend;
    private GameObject aboutTab, controlsTab, objectsTab, about, controls, objects;

    protected override void Start() {
        base.Start();
        aboutTab = legend.transform.GetChild(0).gameObject;
        controlsTab = legend.transform.GetChild(1).gameObject;
        objectsTab = legend.transform.GetChild(2).gameObject;
        about = legend.transform.GetChild(3).gameObject;
        controls = legend.transform.GetChild(4).gameObject;
        objects = legend.transform.GetChild(5).gameObject;
    }

    public void ToggleInfo () {
        if (!legend.activeSelf) {
            PlaySE();
            legend.SetActive(true);
        }
        else {
            GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
            legend.SetActive(false);
        }
    }

    public void AboutTab() {
        PlaySE();
        //Disable other tabs
        ColorBlock colorBlock = aboutTab.GetComponent<Button>().colors;
        colorBlock.normalColor = aboutTab.GetComponent<Button>().colors.pressedColor;
        aboutTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = controlsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f/255f, 212f/255f, 212f/255f);
        controlsTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = objectsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f / 255f, 212f / 255f, 212f / 255f);
        objectsTab.GetComponent<Button>().colors = colorBlock;

        about.SetActive(true);
        controls.SetActive(false);
        objects.SetActive(false);
    }

    public void ControlsTab() {
        PlaySE();
        //Disable other tabs
        ColorBlock colorBlock = aboutTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f / 255f, 212f / 255f, 212f / 255f);
        aboutTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = controlsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = controlsTab.GetComponent<Button>().colors.pressedColor;
        controlsTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = objectsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f / 255f, 212f / 255f, 212f / 255f);
        objectsTab.GetComponent<Button>().colors = colorBlock;

        about.SetActive(false);
        controls.SetActive(true);
        objects.SetActive(false);
    }

    public void ObjectsTab() {
        PlaySE();
        //Disable other tabs
        ColorBlock colorBlock = aboutTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f / 255f, 212f / 255f, 212f / 255f);
        aboutTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = controlsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(207f / 255f, 212f / 255f, 212f / 255f);
        controlsTab.GetComponent<Button>().colors = colorBlock;

        colorBlock = objectsTab.GetComponent<Button>().colors;
        colorBlock.normalColor = aboutTab.GetComponent<Button>().colors.pressedColor;
        objectsTab.GetComponent<Button>().colors = colorBlock;

        about.SetActive(false);
        controls.SetActive(false);
        objects.SetActive(true);
    }
}
