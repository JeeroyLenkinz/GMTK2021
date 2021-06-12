using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedPieces : MonoBehaviour
{
    bool firstFrame = true;

    private void Awake()
    {
        StartCoroutine(fadeMeOutCoroutine());
    }

    private void LateUpdate()
    {
        firstFrame = false;
    }

    public void OnCollisionEnter2D(Collision collision)
    {
        if (collision.transform.tag == "Player" && firstFrame)
        {
            StartCoroutine(fadeMeOutNow());
        }
    }

    private IEnumerator fadeMeOutNow()
    {
        Tween tween = GetComponent<SpriteRenderer>().DOFade(0f, 1f);
        yield return new WaitForSeconds(tween.Duration());
        Destroy(this.gameObject);
    }

    private IEnumerator fadeMeOutCoroutine()
    {
        yield return new WaitForSeconds(5);
        Tween tween = GetComponent<SpriteRenderer>().DOFade(0f, 1f);
        yield return new WaitForSeconds(tween.Duration());
        Destroy(this.gameObject);
    }
}
