using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ScreenShake : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerShake()
    {
        Debug.Log("Enemy SHAKE");
        CameraShaker.Instance.ShakeOnce(10f, 4f, 0.1f, 0.5f);
    }

    public void TriggerShakeSmall()
    {
        Debug.Log("Enemy SHAKE");
        CameraShaker.Instance.ShakeOnce(1f, 4f, 0.1f, 0.5f);
    }

}
