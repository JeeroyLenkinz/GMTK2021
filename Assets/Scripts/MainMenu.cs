using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameEvent menuFadeOut;

    public void PlayGame()
    {
        StartCoroutine(PlayGameCo());
    }

    private IEnumerator PlayGameCo()
    {
        menuFadeOut.Raise();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main");
    }

    public void LoadTutorial()
    {
        StartCoroutine(TutorialCo());

    }

    private IEnumerator TutorialCo()
    {
        menuFadeOut.Raise();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Tutorial");
    }

    public void QuitToTitle()
    {
        StartCoroutine(QuitTitleCo());
    }

    private IEnumerator QuitTitleCo()
    {
        menuFadeOut.Raise();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Title");
    }


    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}
