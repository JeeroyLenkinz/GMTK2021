using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteGet : MonoBehaviour
{

    Vignette vignette;

    private void Awake()
    {
        VolumeProfile volumeProfile = GetComponent<Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));

        vignette.intensity.Override(0.37f);
    }

    public Vignette GetVignette()
    {
        return vignette;
    }

 
    // You can leave this variable out of your function, so you can reuse it throughout your class.

}
