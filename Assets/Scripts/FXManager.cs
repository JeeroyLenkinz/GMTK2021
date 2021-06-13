using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEditorInternal;

public class FXManager : MonoBehaviour
{
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

    bool isSummoning = false;
    bool isDesummoning = false;

    float summonTimer = 0f;
    float summonEffectDur = 2f;

    float vigIntensity = 0.24f;
    float sat = 10f;

    // Start is called before the first frame update
    void Start()
    {
        vignette = vigScript.GetVignette();
        colorAdjust = vigScript.GetColorAdjust();
        chromaticAbb = vigScript.GetChromaticAbb();

        vignette.intensity.Override(defaultVigIntensity);
        colorAdjust.contrast.Override(10f);
        colorAdjust.saturation.Override(defaultSaturation);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (isSummoning)
        {
            SummonStartFX();
        }
        if (isDesummoning)
        {
            DeSummonStartFX();
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
        isDesummoning = true;
        summonTimer = 0f;
    }

    public void e_Heard_Summon_Start()
    {
        summonTimer = 0f;
        isSummoning = true;
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
            isDesummoning = false;
        }


    }

    private void DeSummonStartFX()
    {


        if (summonTimer < summonEffectDur)
        {
            summonTimer += Time.deltaTime;
            vigIntensity = Mathf.Lerp(vigIntensity, defaultVigIntensity, 0.01f);
            sat = Mathf.Lerp(sat, defaultSaturation, 1f);
            vignette.intensity.Override(vigIntensity);
            colorAdjust.saturation.Override(sat);
        }
        else
        {
            isSummoning = false;
        }


    }

    private IEnumerator MoveToGhostVFX()
    {
        float effectDur = 0.65f;        // How long the cumulative effect is
        cam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        overlaycam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        yield return new WaitForSeconds(effectDur);
        moveToGhostFXDone.Raise();
        yield return null;
    }
}
