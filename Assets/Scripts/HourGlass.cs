using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine.UI;

public class HourGlass : MonoBehaviour
{
    [SerializeField]
    private FloatReference summonHealthSO;

    public Sprite[] hourglassSprites;

    public GameObject HourGlassHolder;

    public Image hourglassImg;

    // Start is called before the first frame update
    void Awake()
    {
        hourglassImg.sprite = hourglassSprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void e_StartChanneling()
    {
        HourGlassHolder.SetActive(true);
    }

    public void e_disableHealthBar()
    {
        HourGlassHolder.SetActive(false);
    }

    public void e_ChangeHealthBar()
    {
        if (summonHealthSO.Value >= 95)
        {
            hourglassImg.sprite = hourglassSprites[1];
        }
        else if (summonHealthSO.Value <= 95 && summonHealthSO.Value > 90)
        {
            hourglassImg.sprite = hourglassSprites[2];
        }
        else if (summonHealthSO.Value <= 90 && summonHealthSO.Value > 85)
        {
            hourglassImg.sprite = hourglassSprites[3];
        }
        else if (summonHealthSO.Value <= 85 && summonHealthSO.Value > 80)
        {
            hourglassImg.sprite = hourglassSprites[4];
        }
        else if (summonHealthSO.Value <= 80 && summonHealthSO.Value > 75)
        {
            hourglassImg.sprite = hourglassSprites[5];
        }
        else if (summonHealthSO.Value <= 75 && summonHealthSO.Value > 70)
        {
            hourglassImg.sprite = hourglassSprites[6];
        }
        else if (summonHealthSO.Value <= 70 && summonHealthSO.Value > 65)
        {
            hourglassImg.sprite = hourglassSprites[7];
        }
        else if (summonHealthSO.Value <= 65 && summonHealthSO.Value > 60)
        {
            hourglassImg.sprite = hourglassSprites[8];
        }
        else if (summonHealthSO.Value <= 60 && summonHealthSO.Value > 55)
        {
            hourglassImg.sprite = hourglassSprites[9];
        }
        else if (summonHealthSO.Value <= 55 && summonHealthSO.Value > 50)
        {
            hourglassImg.sprite = hourglassSprites[10];
        }
        else if (summonHealthSO.Value <= 50 && summonHealthSO.Value > 45)
        {
            hourglassImg.sprite = hourglassSprites[11];
        }
        else if (summonHealthSO.Value <= 45 && summonHealthSO.Value > 40)
        {
            hourglassImg.sprite = hourglassSprites[12];
        }
        else if (summonHealthSO.Value <= 40 && summonHealthSO.Value > 35)
        {
            hourglassImg.sprite = hourglassSprites[13];
        }
        else if (summonHealthSO.Value <= 35 && summonHealthSO.Value > 30)
        {
            hourglassImg.sprite = hourglassSprites[14];
        }
        else if (summonHealthSO.Value <= 30 && summonHealthSO.Value > 25)
        {
            hourglassImg.sprite = hourglassSprites[15];
        }
        else if (summonHealthSO.Value <= 25 && summonHealthSO.Value > 20)
        {
            hourglassImg.sprite = hourglassSprites[16];
        }
        else if (summonHealthSO.Value <= 20 && summonHealthSO.Value > 15)
        {
            hourglassImg.sprite = hourglassSprites[17];
        }
        else if (summonHealthSO.Value <= 15 && summonHealthSO.Value > 10)
        {
            hourglassImg.sprite = hourglassSprites[18];
        }
        else if (summonHealthSO.Value <= 10 && summonHealthSO.Value > 5)
        {
            hourglassImg.sprite = hourglassSprites[19];
        }
        else if (summonHealthSO.Value <= 5 && summonHealthSO.Value > 0)
        {
            hourglassImg.sprite = hourglassSprites[20];
        }
        else if (summonHealthSO.Value <= 0)
        {
            e_disableHealthBar();
        }
    }
}
