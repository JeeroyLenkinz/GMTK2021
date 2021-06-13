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
    public AudioClip dieSFX;
    public AudioClip reconnectToGhostSFX;

    private bool isDead;
    
    void Start()
    {
        isSevered.Value = false;
        isChanneling.Value = false;
        ghost.SetActive(false);
        pChain = GetComponent<PlayerChain>();
        isDead = false;
    }

    public void e_channelTriggered() {
        if (!isDead) {
            if (isChanneling.Value) {
                isMovingToGhost.Value = true;
                stopChanneling.Raise(); // Camera and FX manager hears this, and will trigger an FX done event to do the actual mechanix
            } else if (!isSevered.Value) {
                startChanneling.Raise();
            }
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
        GetComponent<CapsuleCollider2D>().enabled = false;
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
        audioSource.clip = reconnectToGhostSFX;
        audioSource.Play();
    }

    private IEnumerator MoveToGhostCoroutine(List<GameObject> chainedEnemies)
    {
        Tweener tweenb = transform.DOScaleX(4.5f, 0.3f).SetEase(Ease.InBack);
        Tweener tweener = transform.DOScaleY(1.5f, 0.3f).SetEase(Ease.InBack);

        //Tweener tweenb = transform.DOScale(new Vector3(1, 1, 1), 0.3f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(tweenb.Duration());

        Tweener tweena = transform.DOScaleX(1f, 0.35f).SetEase(Ease.InOutBack);
        transform.DOScaleY(3.5f, 0.35f).SetEase(Ease.InOutBack);

        if (chainedEnemies.Count == 0)
        {
            Vector2 direction = (ghost.transform.position - transform.position).normalized;
            DOTween.To(() => (Vector2)transform.up, x => transform.up = x, new Vector2(direction.x, direction.y), 0.3f).SetEase(Ease.InOutBack);
        }
        else
        {
            Vector2 direction = (chainedEnemies[0].transform.position - transform.position).normalized;
            DOTween.To(() => (Vector2)transform.up, x => transform.up = x, new Vector2(direction.x, direction.y), 0.3f).SetEase(Ease.InOutBack);
        }

        yield return new WaitForSeconds(tweena.Duration());

        if (chainedEnemies.Count == 0)
        {
            // Move to ghost
            float distance = (ghost.transform.position - transform.position).magnitude;
            float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations

            Tween tween = transform.DOMove(ghost.transform.position, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween.Duration());
            Tweener tweenc = transform.DOScaleX(3f, 0.3f).SetEase(Ease.InOutBack, 3f);
            transform.DOScaleY(3f, 0.3f).SetEase(Ease.InOutBack, 3f);

            DOTween.To(() => (Vector2)transform.up, x => transform.up = x, Vector2.zero, 0.5f);
            EndOfDash();
        }
        else
        {
            int index = 0;
            foreach (GameObject enemy in chainedEnemies)
            {
                pChain.AttachNext(enemy);
                float distance = (enemy.transform.position - transform.position).magnitude;
                float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations


                Tween tween = transform.DOMove(enemy.transform.position, duration).SetEase(Ease.Linear);
                Vector2 directionb = (enemy.transform.position - transform.position).normalized;
                //DOTween.To(() => (Vector2)transform.up, x => transform.up = x, new Vector2(directionb.x, directionb.y), 0.001f).SetEase(Ease.Linear);
                transform.up = directionb;
                yield return new WaitForSeconds(tween.Duration());

                //Destroy(enemy);
                enemy.GetComponent<Enemy>().Explode();
                enemyDestroyedEvent.Raise();
                index++;
            }

            // Move to ghost
            pChain.AttachNext(ghost);
            float distance2 = (ghost.transform.position - transform.position).magnitude;
            float duration2 = distance2 / moveToGhostSpeed;       // Because DOTween operates off durations

            Vector2 directionc = (ghost.transform.position - transform.position).normalized;
            DOTween.To(() => (Vector2)transform.up, x => transform.up = x, new Vector2(directionc.x, directionc.y), duration2 / 4).SetEase(Ease.Linear);

            Tween tween2 = transform.DOMove(ghost.transform.position, duration2).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween2.Duration());

            Tweener tweenc = transform.DOScaleX(3f, 0.3f).SetEase(Ease.InOutBack, 3f);
            transform.DOScaleY(3f, 0.3f).SetEase(Ease.InOutBack, 3f);

            DOTween.To(() => (Vector2)transform.up, x => transform.up = x, Vector2.zero, 0.3f).SetEase(Ease.Linear);

            EndOfDash();
        }
    }

    private void EndOfDash()
    {
        GetComponent<LineRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
        ghost.GetComponent<GhostManager>().StopWaiting();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ghost" && isSevered.Value) {
            reconnectToGhost();
        }
    }

    public void e_getHit() {
        if (!isMovingToGhost.Value && !isDead) 
        {
            if (isChanneling.Value) {
                // Initiate sever
                severConnectionEvent.Raise();
                ghost.GetComponent<GhostManager>().StartSever();
            } else {
                // Die
                isDead = true;
                gameOverEvent.Raise();
                disableMovement();
                audioSource.clip = dieSFX;
                audioSource.Play();
            }
        }
    }
}
