using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class HumanManager : PlayerManager
{
    [SerializeField]
    private GameObject ghost;
    [SerializeField]
    private GameEvent enemyDestroyedEvent;

    public float moveToGhostSpeed;

    private PlayerChain pChain;

    void Start()
    {
        ghost.SetActive(false);
        pChain = GetComponent<PlayerChain>();
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

        if (chainedEnemies.Count == 0)
        {
            // Move to ghost
            float distance = (ghost.transform.position - transform.position).magnitude;
            float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations

            Tween tween = transform.DOMove(ghost.transform.position, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween.Duration());
            EndOfDash();
        }
        else
        {
            foreach (GameObject enemy in chainedEnemies)
            {
                pChain.AttachNext(enemy);
                float distance = (enemy.transform.position - transform.position).magnitude;
                float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations

                Tween tween = transform.DOMove(enemy.transform.position, duration).SetEase(Ease.Linear);
                yield return new WaitForSeconds(tween.Duration());

                Destroy(enemy);
                enemyDestroyedEvent.Raise();

            }

            // Move to ghost
            pChain.AttachNext(ghost);
            float distance2 = (ghost.transform.position - transform.position).magnitude;
            float duration2 = distance2 / moveToGhostSpeed;       // Because DOTween operates off durations

            Tween tween2 = transform.DOMove(ghost.transform.position, duration2).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween2.Duration());

            EndOfDash();
        }
    }

    private void EndOfDash()
    {
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        ghost.GetComponent<GhostManager>().StopWaiting();
    }
}
