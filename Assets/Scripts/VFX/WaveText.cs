using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveText : MonoBehaviour
{

    public TextMeshProUGUI text;
    private AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    public void e_New_Wave_Text(int waveNum)
    {
        StartCoroutine(newWave(waveNum));
    }

    public void e_start_message()
    {
        StartCoroutine(startMessage());
    }

    private IEnumerator newWave(int waveNum)
    {
        text.fontSize = 300;
        text.DOFade(1f, 0.15f);
        audio.Play();
        text.text = "WAVE";
        yield return new WaitForSeconds(1f);
        audio.Play();
        text.text = waveNum.ToString();
        yield return new WaitForSeconds(1f);
        text.DOFade(0f, 0.15f);
    }

    private IEnumerator startMessage()
    {
        text.DOFade(1f, 0.15f);
        text.fontSize = 250;
        audio.Play();
        text.text = "Survive";
        yield return new WaitForSeconds(1f);
        audio.Play();
        text.text = 5.ToString();
        yield return new WaitForSeconds(1f);
        audio.Play();
        text.text = "Waves";
        yield return new WaitForSeconds(1f);
        text.DOFade(0f, 0.15f);
    }
}
