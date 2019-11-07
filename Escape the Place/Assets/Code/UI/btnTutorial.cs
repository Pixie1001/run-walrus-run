using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnTutorial : ButtonType
{
    public GameObject popup;

    public void Next1() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
        popup.transform.GetChild(1).gameObject.SetActive(true);
        popup.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Next2() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
        popup.transform.GetChild(2).gameObject.SetActive(true);
        popup.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void Next3() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
        SceneManager.LoadScene("Puzzle 1");
    }
}