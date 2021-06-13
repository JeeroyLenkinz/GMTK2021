using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.Experimental.Rendering.Universal;

public class GhostManager : PlayerManager
{
    public AudioClip startChannelSFX;
    public float startChannelSFXVolume;
    public AudioClip endChannelSFX;
    public float endChannelSFXVolume;
    public AudioClip severConnectionSFX;
    public float severConnectionSFXVolume;
    [SerializeField]
    private FloatReference channelHealthSO;
    [SerializeField]
    private GameObjectGameEvent enemyHit;
    [SerializeField]
    private GameEvent severConnectionEvent;
    [SerializeField]
    private GameEvent ghostReachedEvent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private BoolReference isChanneling;

    private List<GameObject> chainedEnemies = new List<GameObject>();
    private bool isWaiting;
    [SerializeField]
    private BoolReference isSevered;
    [SerializeField]
    private BoolReference isMovingToGhost;
    [SerializeField]
    private AudioSource markEnemyAudioSource;

    public float healthDecayMod;
    [SerializeField]
    private GameEvent enemyDestroyedEvent;

    private new void Awake()
    {
        base.Awake();
        isWaiting = false;
        isSevered.Value = false;
        isMovingToGhost.Value = false;
        chainedEnemies.Clear();

    }

    private void OnEnable()
    {
        channelHealthSO.Value = 100;
        audioSource.clip = startChannelSFX;
        audioSource.volume = startChannelSFXVolume;
        audioSource.Play();
    }

    private new void Update()
    {
        base.Update();
        channelHealthSO.Value -= Time.deltaTime*healthDecayMod;
        channelHealthSO.Value = Mathf.Clamp(channelHealthSO.Value, 0, 100);
        if(channelHealthSO.Value == 0 && !isSevered.Value && !isMovingToGhost.Value)
        {
            StartSever();
        }
    }

    public void e_StopChanneling() {
        if (!getIsDashing() && !isSevered.Value) {
            StartCoroutine(StopChannelCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" && !isSevered.Value && !isMovingToGhost.Value)
        {
            GameObject enemy = collision.gameObject;
            Enemy enemyLogic = enemy.GetComponent<Enemy>();

            if(enemyLogic.GetLight() != null)
            {
                enemyLogic.GetLight().gameObject.SetActive(true);
            }


            if(chainedEnemies.Count == 0)             // If this is the first enemy hit
            {
                player.GetComponent<PlayerChain>().AttachNext(enemy);
                chainedEnemies.Add(enemy);
                enemyHit.Raise(enemy);
                enemyLogic.AttachNext(this.gameObject);
            }

            if (!enemyLogic.GetIsChained())     // If enemy is not already in the list
            {
                Vector2 collidedPoint = collision.ClosestPoint(transform.position);
                enemyLogic.SetExplosionOrigin(collidedPoint);
                chainedEnemies[chainedEnemies.Count-1].GetComponent<Enemy>().AttachNext(enemy);
                chainedEnemies.Add(enemy);
                enemyLogic.AttachNext(this.gameObject);
                enemyHit.Raise(enemy);
            }

            markEnemyAudioSource.Play();
            channelHealthSO.Value += 5;

        }
    }

    private IEnumerator StopChannelCoroutine()
    {
        isWaiting = true;
        disableMovement();
        audioSource.clip = endChannelSFX;
        audioSource.volume = endChannelSFXVolume;
        audioSource.Play();
        player.GetComponent<HumanManager>().MoveToGhost(chainedEnemies);
        while (isWaiting)       // Waiting for the movement to end
        {
            yield return null;
        }
        foreach (GameObject killedEnemy in chainedEnemies)
        {
            Destroy(killedEnemy);
            // enemyDestroyedEvent.Raise();
        }
        ghostReachedEvent.Raise();
        chainedEnemies.Clear();
        player.GetComponent<PlayerManager>().enableMovement();
        enableMovement();
        gameObject.SetActive(false);
        isChanneling.Value = false;
        isMovingToGhost.Value = false;
    }

    public void StartSever() {
        isSevered.Value = true;
        isChanneling.Value = false;
        audioSource.clip = severConnectionSFX;
        audioSource.volume = severConnectionSFXVolume;
        audioSource.Play();
        StartCoroutine(SeverConnection());
    }
    private IEnumerator SeverConnection()
    {
        disableMovement();
        severConnectionEvent.Raise();         // Will move Camera to Player - Have lines fade
        chainedEnemies.Clear();
        yield return new WaitForSeconds(0.25f);
        // Do FX stuff here
        player.GetComponent<PlayerManager>().enableMovement();
        // Start pulsing or looking lost or whatever
        yield return null;
    }

    public void StopWaiting()
    {
        isWaiting = false;
    }


}
