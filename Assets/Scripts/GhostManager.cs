using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GhostManager : PlayerManager
{
    [SerializeField]
    private FloatReference channelHealthSO;
    [SerializeField]
    private GameObjectGameEvent enemyHit;
    [SerializeField]
    private GameEvent severConnectionEvent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private BoolReference isChanneling;

    private List<GameObject> chainedEnemies = new List<GameObject>();
    private bool isWaiting;
    [SerializeField]
    private BoolReference isSevered;

    public float healthDecayMod;

    private new void Awake()
    {
        base.Awake();
        isWaiting = false;
        isSevered.Value = false;
        chainedEnemies.Clear();

    }

    private void OnEnable()
    {
        channelHealthSO.Value = 100;
    }

    private new void Update()
    {
        base.Update();
        channelHealthSO.Value -= Time.deltaTime*healthDecayMod;
        channelHealthSO.Value = Mathf.Clamp(channelHealthSO.Value, 0, 100);
        if(channelHealthSO.Value == 0 && !isSevered.Value)
        {
            isSevered.Value = true;
            isChanneling.Value = false;
            chainedEnemies.Clear();
            StartCoroutine(SeverConnection());
        }
    }

    public void e_StopChanneling() {
        if (!getIsDashing() && !isSevered.Value) {
            StartCoroutine(StopChannelCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            GameObject enemy = collision.gameObject;
            Enemy enemyLogic = enemy.GetComponent<Enemy>();

            if(chainedEnemies.Count == 0)             // If this is the first enemy hit
            {
                player.GetComponent<PlayerChain>().AttachNext(enemy);
                chainedEnemies.Add(enemy);
                enemyLogic.AttachNext(this.gameObject);
            }

            if (!enemyLogic.GetIsChained())     // If enemy is not already in the list
            {
                chainedEnemies[chainedEnemies.Count-1].GetComponent<Enemy>().AttachNext(enemy);
                chainedEnemies.Add(enemy);
                enemyLogic.AttachNext(this.gameObject);
                enemyHit.Raise(enemy);
            }

            channelHealthSO.Value += 5;

        }
    }

    private IEnumerator StopChannelCoroutine()
    {
        isWaiting = true;
        disableMovement();
        player.GetComponent<HumanManager>().MoveToGhost(chainedEnemies);
        while (isWaiting)       // Waiting for the movement to end
        {
            yield return null;
        }
        chainedEnemies.Clear();
        player.GetComponent<PlayerManager>().enableMovement();
        enableMovement();
        gameObject.SetActive(false);
        isChanneling.Value = false;
    }

    private IEnumerator SeverConnection()
    {
        disableMovement();
        severConnectionEvent.Raise();         // Will move Camera to Player - Have lines fade
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
