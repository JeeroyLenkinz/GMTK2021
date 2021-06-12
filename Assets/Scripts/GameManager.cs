using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float waitBeforeTransitionSeconds;
    public void e_GameOver() {
        StartCoroutine(gameOver());
    }

    public void e_Victory() {
        StartCoroutine(victory());
    }

    private IEnumerator gameOver() {
        yield return new WaitForSeconds(waitBeforeTransitionSeconds);
        SceneManager.LoadScene("LoseGame");
    }

    private IEnumerator victory() {
        yield return new WaitForSeconds(waitBeforeTransitionSeconds);
        SceneManager.LoadScene("WinGame");
    }
}
