using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimationSO : ScriptableObject
{

    [Range(0.1f, 5f)]
    public float moveDuration = 1.0f;

    [Tooltip("Not used on movement\nMeant for scale")]
    public float startValue = 0f;
    [Tooltip("Not used on movement\nMeant for scale")]
    public float endValue = 1f;

    public Ease moveEase = Ease.Linear;

    public AnimationTweenType animationTweenType = AnimationTweenType.Move;

    public float overShoot = 0f;

}
