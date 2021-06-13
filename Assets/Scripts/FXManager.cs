using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class FXManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent moveToGhostFXDone;
    [SerializeField]
    private VignetteGet vigScript;

    public Camera cam;

    Vignette vignette;
    ColorAdjustments colorAdjust;
    ChromaticAberration chromaticAbb;

    public float ghostVigIntensity;
    public float defaultVigIntensity;
    public float defaultSaturation;

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
    void Update()
    {
        
    }

    public void e_Heard_Move_To_Ghost()
    {
        StartCoroutine(MoveToGhostVFX());
    }

    public void e_Heard_Ghost_Reached()
    {
        float effectDur = 0.3f;
        cam.gameObject.GetComponent<CameraFollow>().GhostReached(effectDur);
    }

    public void e_Heard_Summon_Start()
    {
        StartCoroutine(SummonStartFX());
    }

    private IEnumerator SummonStartFX()
    {
        float timer = 0f;
        float effectDur = 1f;

        float vigIntensity = (float)vignette.intensity;
        float sat = (float)colorAdjust.saturation;

        DOTween.To(() => vigIntensity, x => vigIntensity = x, ghostVigIntensity, effectDur);
        DOTween.To(() => sat, x => sat = x, 0f, effectDur);
        Debug.Log("About to desat");
        while (timer < effectDur)
        {
            timer += Time.deltaTime;
            vignette.intensity.Override(vigIntensity);
            colorAdjust.saturation.Override(0f);
        }
        Debug.Log("Desat");
        yield return null;

    }

    private IEnumerator MoveToGhostVFX()
    {
        float effectDur = 0.65f;        // How long the cumulative effect is
        cam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        yield return new WaitForSeconds(effectDur);
        moveToGhostFXDone.Raise();
        yield return null;
    }
}
