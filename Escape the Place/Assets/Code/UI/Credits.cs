using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void EndCredits() {
        SceneManager.LoadScene("Title Page");
    }
}
