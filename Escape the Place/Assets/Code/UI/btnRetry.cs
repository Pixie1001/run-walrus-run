using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class btnRetry : ButtonType
{

    private GameObject failScreen;

    // Use this for initialization
    override protected void Start() {
        base.Start();
        failScreen = GameObject.FindGameObjectWithTag("failScreen");
    }

    public void Retry() {
        PlaySE();
        //beginScreen.SetActive(false);
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TileGrid>().pause = false;
        Debug.Log("Click retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
