using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class FXManager : MonoBehaviour
{
    private enum EffectState
    {
        Normal,
        Summoned,
        Severed
    }
    private EffectState effectState;

    [SerializeField]
    private GameEvent moveToGhostFXDone;
    [SerializeField]
    private VignetteGet vigScript;

    public Camera cam;
    public Camera overlaycam;

    Vignette vignette;
    ColorAdjustments colorAdjust;
    ChromaticAberration chromaticAbb;

    public float ghostVigIntensity;
    public float defaultVigIntensity;
    public float defaultSaturation;
    public float SeverAbIntensity;
    public float SeverSat;

    bool isSummoning = false;
    bool isDesummoning = false;
    bool isSevering = false;
    bool isReattaching = false;

    float summonTimer = 0f;
    float summonEffectDur = 2f;

    float severTimer = 0f;
    float severEffectDur = 2f;

    float vigIntensity = 0.24f;
    float sat = 10f;
    float chromaticAbIntensity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        vignette = vigScript.GetVignette();
        colorAdjust = vigScript.GetColorAdjust();
        chromaticAbb = vigScript.GetChromaticAbb();

        vignette.intensity.Override(defaultVigIntensity);
        colorAdjust.contrast.Override(10f);
        colorAdjust.saturation.Override(defaultSaturation);
        chromaticAbb.intensity.Override(0f);

        effectState = EffectState.Normal;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isSummoning)
        {
            SummonStartFX();
        }
        else if (isDesummoning)
        {
            DeSummonStartFX();
        } 
        if (isSevering)
        {
            SevereStartFX();
        }
        else if (isReattaching)
        {
            ReAttachStartFX();
        }
    }

    public void e_Heard_Move_To_Ghost()
    {
        StartCoroutine(MoveToGhostVFX());
    }

    public void e_Heard_Ghost_Reached()
    {
        float effectDur = 0.3f;
        cam.gameObject.GetComponent<CameraFollow>().GhostReached(effectDur);
        overlaycam.gameObject.GetComponent<CameraFollow>().GhostReached(effectDur);
        isSummoning = false;
        isDesummoning = true;
        summonTimer = 0f;
        effectState = EffectState.Normal;
    }

    public void e_Heard_Summon_Start()
    {
        summonTimer = 0f;
        isSummoning = true;
        effectState = EffectState.Summoned;
    }

    public void e_Heard_Sever_Start()
    {
        isReattaching = false;
        summonTimer = 0f;
        severTimer = 0f;
        if(effectState == EffectState.Summoned)
        {
            isSummoning = false;
            isDesummoning = true;
        }
        isSevering = true;
        effectState = EffectState.Severed;
    }

    public void e_Heard_Reattach_Start()
    {

        severTimer = 0f;
        isReattaching = true;
        isSevering = false;
        effectState = EffectState.Normal;
    }

    private void SummonStartFX()
    {


        if (summonTimer < summonEffectDur)
        {
            summonTimer += Time.deltaTime;
            vigIntensity = Mathf.Lerp(vigIntensity, ghostVigIntensity, 0.01f);

            sat = Mathf.Lerp(sat, -100f, 0.05f);

            vignette.intensity.Override(vigIntensity);
            colorAdjust.saturation.Override(sat);
        }
        else
        {
            isSummoning = false;
        }


    }

    private void DeSummonStartFX()
    {
        if (summonTimer < summonEffectDur)
        {
            summonTimer += Time.deltaTime;
            vigIntensity = Mathf.Lerp(vigIntensity, defaultVigIntensity, 0.01f);
            if (effectState == EffectState.Severed)
            {
                sat = Mathf.Lerp(sat, SeverSat, 2f);
            }
            else
            {
                sat = Mathf.Lerp(sat, defaultSaturation, 1f);
            }
            vignette.intensity.Override(vigIntensity);
            colorAdjust.saturation.Override(sat);


        }
        else
        {
            isDesummoning = false;
        }
    }

    private void SevereStartFX()
    {
        if (severTimer < severEffectDur)
        {
            severTimer += Time.deltaTime;
            chromaticAbIntensity = Mathf.Lerp(chromaticAbIntensity, SeverAbIntensity, 0.05f);
            chromaticAbb.intensity.Override(chromaticAbIntensity);
        }
        else
        {
            isSevering = false;
        }
    }

    private void ReAttachStartFX()
    {
        if (severTimer < severEffectDur)
        {
            severTimer += Time.deltaTime;
            chromaticAbIntensity = Mathf.Lerp(chromaticAbIntensity, 0f, 0.05f);
            sat = Mathf.Lerp(sat, defaultSaturation, 1f);
            chromaticAbb.intensity.Override(chromaticAbIntensity);
            colorAdjust.saturation.Override(sat);
        }
        else
        {
            isSevering = false;
        }
    }

    private IEnumerator MoveToGhostVFX()
    {
        float effectDur = 0.4f;        // How long the cumulative effect is
        cam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        overlaycam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        //yield return new WaitForSeconds(effectDur/2); // removed to reduce anim time
        moveToGhostFXDone.Raise();
        yield return null;
    }
}
