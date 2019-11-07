using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnInfo: ButtonType
{
    public GameObject legend;

    public void ToggleInfo () {
        PlaySE();
        if (!legend.activeSelf) {
            legend.SetActive(true);
        }
        else {
            legend.SetActive(false);
        }
    }


}
