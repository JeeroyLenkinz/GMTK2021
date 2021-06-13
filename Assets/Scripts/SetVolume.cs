using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    void Start() {
        if (SceneManager.GetActiveScene().name == "Title") {
            slider.value = 1f;
        } else {
            slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        }
    }

    public void SetLevel(float sliderValue) {
        Debug.Log("SetLevel was called! The float val is: " + sliderValue);
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }
}
