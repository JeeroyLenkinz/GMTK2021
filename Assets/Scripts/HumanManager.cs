using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class HumanManager : PlayerManager
{
    public AudioClip dieSFX;
    public float dieSFXVolume;
    public AudioClip reconnectToGhostSFX;
    public float reconnectToGhostSFXVolume;
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
    public float ghostSpawnOffset;

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
    [SerializeField]
    private GameEvent reConnectToGhostEvent;
    private bool isDead;

    public GameObject ArtBigGO;
    public GameObject trailGO;
    private bool isInvincible;
    public float invincibilityDurationSeconds;
    [SerializeField]
    private List<SpriteRenderer> artAssets = new List<SpriteRenderer>();

    void Start()
    {
        isSevered.Value = false;
        isChanneling.Value = false;
        ghost.SetActive(false);
        pChain = GetComponent<PlayerChain>();
        isDead = false;
        isInvincible = false;
    }

    public void e_channelTriggered() {
        if (!isDead && !getIsDashing()) {
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
            ghost.transform.position = gameObject.transform.position + (lastMoveDir * ghostSpawnOffset);
            if (ghost.transform.position.x < gameObject.transform.position.x) {
                ghost.transform.localScale = new Vector2(-3, 3);
            }
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
        reConnectToGhostEvent.Raise();
        isSevered.Value = false;
        ghost.SetActive(false);
        audioSource.clip = reconnectToGhostSFX;
        audioSource.volume = reconnectToGhostSFXVolume;
        audioSource.Play();
    }

    private IEnumerator MoveToGhostCoroutine(List<GameObject> chainedEnemies)
    {
        Tweener tweenb = transform.DOScaleX(5.5f, 0.25f).SetEase(Ease.InBack);
        Tweener tweener = transform.DOScaleY(1.5f, 0.25f).SetEase(Ease.InBack);

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

        transform.DOScale(Vector3.zero, 0.2f);  // Set to zero
        ArtBigGO.SetActive(false);
        transform.localScale = new Vector3(1f, 3.5f, 3f); //reenable size for collisions and shit - nums what it is before zeroing it
        trailGO.SetActive(true);

        if (chainedEnemies.Count == 0)
        {
            // Move to ghost
            float distance = (ghost.transform.position - transform.position).magnitude;
            float duration = distance / moveToGhostSpeed;       // Because DOTween operates off durations

            Tween tween = transform.DOMove(ghost.transform.position, duration).SetEase(Ease.Linear);
            yield return new WaitForSeconds(tween.Duration());
            transform.up = Vector2.zero;
            trailGO.SetActive(false);
            ArtBigGO.SetActive(true);
            Tweener tweenc = transform.DOScaleX(3f, 0.3f).SetEase(Ease.InOutBack, 3f);
            transform.DOScaleY(3f, 0.3f).SetEase(Ease.InOutBack, 3f);

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
                // enemyDestroyedEvent.Raise();
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

            transform.up = Vector2.zero;
            trailGO.SetActive(false);
            ArtBigGO.SetActive(true);
            Tweener tweenc = transform.DOScaleX(3f, 0.3f).SetEase(Ease.InOutBack, 3f);
            transform.DOScaleY(3f, 0.3f).SetEase(Ease.InOutBack, 3f);

            EndOfDash();
        }
    }
    private void EndOfDash()
    {
        GetComponentInChildren<LineRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = true;
        ghost.GetComponent<GhostManager>().StopWaiting();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ghost" && isSevered.Value) {
            reconnectToGhost();
        }
    }

    public void e_getHit() {
        if (!isMovingToGhost.Value && !isDead && !isInvincible) 
        {
            if (isChanneling.Value) {
                // Initiate sever
                severConnectionEvent.Raise();
                StartCoroutine(setInvincible());
                ghost.GetComponent<GhostManager>().StartSever();
            } else {
                // Die
                isDead = true;
                gameOverEvent.Raise();
                disableMovement();
                audioSource.clip = dieSFX;
                audioSource.volume = dieSFXVolume;
                audioSource.Play();
            }
        }
    }

    private IEnumerator setInvincible() {
        isInvincible = true;
        foreach (SpriteRenderer sprite in artAssets) {
            sprite.DOFade(0.3f, 0.75f);
        }
        yield return new WaitForSeconds(invincibilityDurationSeconds - 0.75f);
        foreach (SpriteRenderer sprite in artAssets) {
            sprite.DOFade(1f, 0.75f);
        }
        yield return new WaitForSeconds(0.75f);
        isInvincible = false;
    }
}
