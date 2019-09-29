using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnInfo: MonoBehaviour
{
    public GameObject legend;

    protected void Start() {
    }

    public void ToggleInfo () {
        if (!legend.activeSelf) {
            legend.SetActive(true);
        }
        else {
            legend.SetActive(false);
        }
    }
}
