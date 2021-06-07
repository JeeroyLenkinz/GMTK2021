using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// With this outside the class, it should be able
// to be accessed just like it's in this script
public enum AnimationTweenType
{
    // Add new ones here to correspond to new SOs
    Move,
    Move2,
    Scale,
    RotateZ,
    RotateZ2,
    FadeOutText,
    EndMove,
    FadeRadio
}

public class Animate_Basic : MonoBehaviour
{
    // This is a class meant to be attached to GOs
    // It should handle all generic animations
    // Then its public functions are called by other scripts

    /*
     * So here's how to use this if it has been a while.
     * You must also take the Animation SO script.
     * 
     * Think of the SOs, as creating re-useable parameters for your animations (excluding start/end)
     * Step 1) Create SO
     * 
     * 2) Create a value in the enum above by a similar name
     * 
     * 3) Change the values in the SO to what you want (if you don't need a value, like JumpPow, just ignore it)
     * 
     * 4) Populate the startLocations, targetLocations, and animationSOs in the same order
     *      i.e. if you make a "MoveStraight" animation, make sure it's target and animation have the same index
     *      If an animation's start position is relative, you can leave it blank
     *      
     * The point of this, is that you might want multiple objects to have the same "grow animation" and this should make it easier.
     * 
     * UPDATE V1.1
     * Removed positional list, instead you pass the Vector 2 to which you want it to start or end.
     * 
     * If it does need a start position, but is passed a Vector2.Zero, then it will be a relative position
     * 
     * If it does not start anywhere, use Vector2.zero as param
     */

    #region DebugVars

    public Transform ResetTrans;

    #endregion



    // This must be the same length as targetLocations because their indexes must correspond.
    [SerializeField]
    private AnimationSO[] animationSOs;


    // TODO remove array BECAUSE ITS MESSING THINGS UP
    // AND REMOVE THE INTIALIZING PART OF THE MAIN METHOD

    void Start()
    {

        // We need to make sure two aren't of the same type because that wouldn't make sense
        foreach (AnimationSO animSO in animationSOs)
        {
            foreach (AnimationSO animSO2 in animationSOs)
            {
                if(animSO.animationTweenType == animSO2.animationTweenType && animSO != animSO2)
                {
                    Debug.LogError("Two animations are of the same type.");
                }
            }
        }
    }

    public Tweener Animate(AnimationTweenType localAnimationTweenType, Vector2 startPoint, Vector2 endPoint)
    {

        // Initializing and finding correct SO ------------------

        AnimationSO animSO = animationSOs[0];
        int arrayIndex = 0;
        float overShoot = 1.701f; // default value

        // This loop finds the given animation that corresponds to
        // the desired type as given by the parameter enum
        for (int i = 0; i < animationSOs.Length; i++)
        {
            AnimationSO tempSO = animationSOs[i];

            if(tempSO.animationTweenType == localAnimationTweenType)
            {
                animSO = tempSO;
                arrayIndex = i;
                if(tempSO.overShoot == 0f)
                {
                    overShoot = DOTween.defaultEaseOvershootOrAmplitude;
                }
                else
                {
                    overShoot = tempSO.overShoot;
                }
                break;
            }
        }

        // End initialization -------------------------

        switch (localAnimationTweenType)
        {
            case AnimationTweenType.Move:
                //startPoint = checkVectVals(startPoint);
                return transform.DOMove(endPoint, animSO.moveDuration).SetEase(animSO.moveEase, overShoot);
                break;

            case AnimationTweenType.Move2:
                //startPoint = checkVectVals(startPoint);
                return transform.DOMove(endPoint, animSO.moveDuration).SetEase(animSO.moveEase, overShoot);
                break;

            case AnimationTweenType.Scale:
                transform.localScale = new Vector3(animSO.startValue, animSO.startValue, animSO.startValue);
                return transform.DOScale(animSO.endValue, animSO.moveDuration).SetEase(animSO.moveEase, overShoot);
                break;

            case AnimationTweenType.RotateZ:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, animSO.startValue);
                Vector3 endValue = new Vector3(transform.rotation.x, transform.rotation.y, animSO.endValue);
                return transform.DOLocalRotate(endValue, animSO.moveDuration).SetEase(animSO.moveEase, overShoot);
                break;


            case AnimationTweenType.FadeOutText:
                TextMeshProUGUI myText = GetComponent<TextMeshProUGUI>();
                return myText.DOFade(0f, animSO.moveDuration).SetEase(animSO.moveEase);

            default:
                return null;
                break;
        }
        
    }

    // If the animate method is given Vector2.zero, it should be a relative animation start point
    // This just checks that and then returns the objects position if it is zero
    private Vector2 checkVectVals(Vector2 startPoint)
    {
        if (startPoint != Vector2.zero)
        {
            return startPoint;
        }
        else
        {
            return transform.position;
        }
    }

    #region Debug

    public void e_Reset()
    {
        transform.position = ResetTrans.position;
        transform.localScale = ResetTrans.localScale;
    }

    #endregion
}

