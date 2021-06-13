using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class HourGlass : MonoBehaviour
{
    [SerializeField]
    private FloatReference summonHealthSO;

    public RectTransform hourglass;
    public RectTransform sand;
    public RectTransform startPos;
    public RectTransform endPos;
    public GameObject HourGlassHolder;

    // Start is called before the first frame update
    void Start()
    {
        
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
        if (summonHealthSO.Value > 0)
        {
            //float normalizedMove = ((startPos.rect.position.y - endPos.rect.position.y) * summonHealthSO.Value) + endPos.rect.position.y;
            sand.localPosition = new Vector3(sand.rect.position.x, (100 - summonHealthSO.Value)*-1);
        }
        else
        {
            e_disableHealthBar();
        }
    }
}
