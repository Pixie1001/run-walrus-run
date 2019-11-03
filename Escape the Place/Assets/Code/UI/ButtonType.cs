using UnityEngine;
using System.Collections;

public class ButtonType : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip soundEffects;

    protected virtual void Start() {
        audioSource = (AudioSource)gameObject.AddComponent<AudioSource>();
        soundEffects = Resources.Load<AudioClip>("Audio/Upload/UISelect");
    }

    public void PlaySE() {
        audioSource.PlayOneShot(soundEffects);
    }
}
