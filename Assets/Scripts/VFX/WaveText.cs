using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveText : MonoBehaviour
{
    bool isDisplaying;

    public TextMeshProUGUI text;
    private AudioSource audio;

    private void Awake()
    {
        isDisplaying = false;
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

    public void e_severed()
    {
        if (!isDisplaying)
        {
            StartCoroutine(severedMessage());
        }

    }

    private IEnumerator newWave(int waveNum)
    {
        isDisplaying = true;
        text.fontSize = 300;
        text.DOFade(1f, 0.15f);
        audio.Play();
        text.text = "WAVE";
        yield return new WaitForSeconds(1f);
        audio.Play();
        text.text = waveNum.ToString();
        yield return new WaitForSeconds(1f);
        text.DOFade(0f, 0.15f);
        isDisplaying = false;
    }

    private IEnumerator startMessage()
    {
        isDisplaying = true;
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
        isDisplaying = false;
    }

    private IEnumerator severedMessage()
    {
        isDisplaying = true;
        text.fontSize = 300;
        text.DOFade(1f, 0.15f);
        audio.Play();
        text.text = "FIND";
        yield return new WaitForSeconds(1f);
        audio.Play();
        text.text = "IT!";
        yield return new WaitForSeconds(1f);
        text.DOFade(0f, 0.15f);
        isDisplaying = false;
    }
}
