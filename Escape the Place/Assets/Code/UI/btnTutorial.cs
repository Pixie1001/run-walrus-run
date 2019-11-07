using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class btnTutorial : ButtonType
{
    public GameObject popup;

    public void Close() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
        SceneManager.LoadScene("Puzzle 1");
    }

    public void Next() {
        GameObject.FindGameObjectWithTag("CloseSE").GetComponent<ButtonType>().PlaySE();
        popup.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}