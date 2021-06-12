using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void e_GameOver() {
        SceneManager.LoadScene("LoseGame");
    }

    public void e_Victory() {
        SceneManager.LoadScene("WinGame");
    }
}
