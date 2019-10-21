using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class btnRetry : MonoBehaviour
{

    private GameObject failScreen;

    // Use this for initialization
    void Start() {
        failScreen = GameObject.FindGameObjectWithTag("failScreen");
    }

    public void Retry() {
        //beginScreen.SetActive(false);
        //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<TileGrid>().pause = false;
        Debug.Log("Click retry");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
