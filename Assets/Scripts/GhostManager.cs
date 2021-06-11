using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class GhostManager : MonoBehaviour
{
    [SerializeField]
    private GameObjectGameEvent enemyHit;

    private void Awake()
    {
        
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
            if (!enemyLogic.GetIsChained())     // If enemy is not already in the list
            {
                enemyLogic.SetIsChained(true);
                enemyHit.Raise(enemy);
            }

        }
    }
}
