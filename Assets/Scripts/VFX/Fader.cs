using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour
{

    public Image blackness;

    private void Awake()
    {
        e_Fade_In();
        if (SceneManager.GetActiveScene().ToString() != "Title")
        {

        }
    }

    public void e_Fade_In()
    {
        StartCoroutine(fadeInCoroutine());
    }

    public void e_Fade_Out()
    {
        StartCoroutine(fadeOutCoroutine());
    }

    private IEnumerator fadeInCoroutine()
    {
        if(blackness.gameObject.activeInHierarchy == false)
        {
            blackness.gameObject.SetActive(true);
        }
        blackness.DOFade(0f, 1.5f);
        yield return new WaitForSeconds(1.5f);
        blackness.gameObject.SetActive(false);
    }

    private IEnumerator fadeOutCoroutine()
    {
        if (blackness.gameObject.activeInHierarchy == false)
        {
            blackness.gameObject.SetActive(true);
        }
        blackness.DOFade(1f, 1.5f);
        yield return new WaitForSeconds(1.5f);
    }

}
