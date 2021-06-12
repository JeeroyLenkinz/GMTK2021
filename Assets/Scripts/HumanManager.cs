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
    [SerializeField]
    private BoolReference isChanneling;
    [SerializeField]
    private GameEvent startChanneling;
    [SerializeField]
    private GameEvent stopChanneling;

    public float moveToGhostSpeed;

    private PlayerChain pChain;
    [SerializeField]
    private BoolReference isSevered;
    [SerializeField]
    private BoolReference isMovingToGhost;
    [SerializeField]
    private GameEvent gameOverEvent;
    [SerializeField]
    private GameEvent severConnectionEvent;

    // private new void Awake() {
    //     base.Awake();
    // }
    
    void Start()
    {
        isSevered.Value = false;
        isChanneling.Value = false;
        ghost.SetActive(false);
        pChain = GetComponent<PlayerChain>();
    }

    public void e_channelTriggered() {
        if (isChanneling.Value) {
            isMovingToGhost.Value = true;
            stopChanneling.Raise(); // Camera and FX manager hears this, and will trigger an FX done event to do the actual mechanix
        } else {
            startChanneling.Raise();
        }
    }

    public void e_startChanneling() {
        if (!getIsDashing() && !isSevered.Value) {
            Debug.Log("Start channeling!");
            isChanneling.Value = true;
            disableMovement();
            ghost.transform.position = gameObject.transform.position;
            ghost.SetActive(true);
            ghost.GetComponent<GhostManager>().enableMovement();
            GetComponent<PlayerChain>().OnSummon();
        }
    }

    public void MoveToGhost(List<GameObject> chainedEnemies)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(MoveToGhostCoroutine(chainedEnemies));
    }

    public void e_SeverConnection()
    {
        StartCoroutine(SeverConnectionCoroutine());
    }

    private IEnumerator SeverConnectionCoroutine()
    {
        pChain.Detach();
        yield return null;
    }

    private void reconnectToGhost() {
        isSevered.Value = false;
        ghost.SetActive(false);
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

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ghost" && isSevered.Value) {
            reconnectToGhost();
        }
    }

    public void e_getHit() {
        if (isChanneling.Value) {
            // Initiate sever
            severConnectionEvent.Raise();
            ghost.GetComponent<GhostManager>().StartSever();
        } else {
            // Die
            gameOverEvent.Raise();
        }
    }
}
