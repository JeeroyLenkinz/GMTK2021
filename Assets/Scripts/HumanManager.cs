using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HumanManager : PlayerManager
{
    [SerializeField]
    private GameObject ghost;
    // Start is called before the first frame update

    public float moveToGhostSpeed;

    void Start()
    {
        ghost.SetActive(false);
    }

    public void e_startChanneling() {
        if (!getIsDashing()) {
            disableMovement();
            ghost.transform.position = gameObject.transform.position;
            ghost.SetActive(true);
            GetComponent<PlayerChain>().OnSummon();
        }
    }

    public void MoveToGhost(List<GameObject> chainedEnemies)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(MoveToGhostCoroutine(chainedEnemies));
    }

    private IEnumerator MoveToGhostCoroutine(List<GameObject> chainedEnemies)
    {
        foreach(GameObject enemy in chainedEnemies)
        {
            Debug.Log("Moving to: " + enemy.transform.name);
            float distance = (enemy.transform.position - transform.position).magnitude;
            float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations

            Tween tween = transform.DOMove(enemy.transform.position, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween.Duration());
        }

        yield return null;
    }

}
