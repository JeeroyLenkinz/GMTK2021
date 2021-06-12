﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

public class UI_SummonHealth : MonoBehaviour
{
    [SerializeField]
    private FloatReference summonHealthSO;

    private Image healthBar;
    private int imageWidth;

    void Awake()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void e_ChangeHealthBar()
    {
        healthBar.rectTransform.localScale = new Vector3(summonHealthSO.Value/100, 1f, 1f);
    }
}
