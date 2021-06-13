using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{

    public GameObject[] livesImages;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject image in livesImages)
        {
            image.SetActive(true); ;
        }
    }


    public void e_UpdateLivesUI(int lives)
    {
        if (lives == 2)
        {
            livesImages[2].SetActive(false);
        }
        else if (lives == 1)
        {
            livesImages[1].SetActive(false);
        }
        else if (lives == 0)
        {
            livesImages[0].SetActive(false);
        }

    }

}
