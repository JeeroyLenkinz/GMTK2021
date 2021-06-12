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
    private GameObject player;

    private List<GameObject> chainedEnemies = new List<GameObject>();
    private bool isWaiting;

    public float healthDecayMod;

    private new void Awake()
    {
        base.Awake();
        isWaiting = false;
        chainedEnemies.Clear();
        channelHealthSO.Value = 100;
    }

    private new void Update()
    {
        base.Update();
        channelHealthSO.Value -= Time.deltaTime*healthDecayMod;
        Mathf.Clamp(channelHealthSO.Value, 0, 100);
        if(channelHealthSO.Value == 0)
        {
            // Sever
        }
    }

    public void e_StopChanneling() {
        if (!getIsDashing()) {
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
    }

    public void StopWaiting()
    {
        isWaiting = false;
    }
}
