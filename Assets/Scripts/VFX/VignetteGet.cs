using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteGet : MonoBehaviour
{

    Vignette vignette;
    ColorAdjustments colorAdjust;
    ChromaticAberration chromaticAbb;

    private void Awake()
    {
        VolumeProfile volumeProfile = GetComponent<Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        if (!volumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
        if (!volumeProfile.TryGet(out colorAdjust)) throw new System.NullReferenceException(nameof(colorAdjust));
        if (!volumeProfile.TryGet(out chromaticAbb)) throw new System.NullReferenceException(nameof(chromaticAbb));
    }

    public Vignette GetVignette()
    {
        return vignette;
    }
    public ColorAdjustments GetColorAdjust()
    {
        return colorAdjust;
    }
    public ChromaticAberration GetChromaticAbb()
    {
        return chromaticAbb;
    }


    // You can leave this variable out of your function, so you can reuse it throughout your class.

}
