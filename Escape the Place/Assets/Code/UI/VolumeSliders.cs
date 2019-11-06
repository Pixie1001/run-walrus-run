using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : ButtonType
{

    public void SFX() {
        float temp = gameObject.GetComponent<Slider>().value;
        if (temp != 1.111111f) {
            OnLoad.sfx = temp;
        }

    }

    public void BGM() {
        float temp = gameObject.GetComponent<Slider>().value;
        if (temp != 1.111111f) {
            OnLoad.bgm = temp;
        }
        GameObject.FindWithTag("MainCamera").GetComponent<TitleScreen>().UpdateAudio();
    }
}
