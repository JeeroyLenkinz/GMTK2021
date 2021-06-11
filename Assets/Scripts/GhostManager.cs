using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GhostManager : MonoBehaviour
{
    [SerializeField]
    private GameObjectGameEvent enemyHit;
    [SerializeField]
    private GameObject player;

    private List<GameObject> chainedEnemies = new List<GameObject>();

    private void Awake()
    {
        chainedEnemies.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

        }
    }
}
