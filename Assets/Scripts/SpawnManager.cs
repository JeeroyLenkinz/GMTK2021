﻿using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpawnManager : MonoBehaviour
{
    private float timeUntilSpawn;
    public float spawnMultiplier;
    private int currentActiveEnemies;
    private int enemiesSpawnedThisWave;
    // public int maxAllowableEnemies;
    public float minSpawnCooldownSeconds;
    public float maxSpawnCooldownSeconds;
    public float waveCooldownSeconds1;
    public float waveCooldownSeconds2;
    public float minEnemySpeed;
    public float maxEnemySpeed;
    public float spawnCircleRadius;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private GameEvent victoryEvent;
    private bool hasWon;
    [SerializeField]
    private IntReference score;
    public int scoreIncrementAmount;
    private int currentWave;
    [SerializeField]
    private List<int> enemiesPerWave = new List<int>();
    private bool isWaitingForWaveStart;
    private AudioSource audioSource;
    public AudioClip killEnemySFX;
    [SerializeField]
    private IntGameEvent waveNumberEvent;
    private bool startOfGame;
    
    // Start is called before the first frame update
    void Awake()
    {
        spawnMultiplier = 1;
        timeUntilSpawn = minSpawnCooldownSeconds;
        score.Value = 0;
        currentWave = 0;
        hasWon = false;
        isWaitingForWaveStart = true;
        enemiesSpawnedThisWave = 0;
        audioSource = GetComponent<AudioSource>();
        startOfGame = true;
    }

    void Start() {
        StartCoroutine(waitBeforeNextWave());
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0 && enemiesSpawnedThisWave < enemiesPerWave[currentWave]  && !isWaitingForWaveStart) {
            spawnEnemy();
            timeUntilSpawn = getSpawnTime();
        }

        if (enemiesSpawnedThisWave >= enemiesPerWave[currentWave] && currentActiveEnemies <= 0 && !isWaitingForWaveStart) {
            if (currentWave < (enemiesPerWave.Count - 1)) {
                StartCoroutine(waitBeforeNextWave());
            } else {
                if (!hasWon) {
                    hasWon = true;
                    victoryEvent.Raise();
                }
            }
        }
    }

    private void spawnEnemy() {
        float enemySpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnCircleRadius;
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        spawnedEnemy.GetComponent<AIDestinationSetter>().target = playerObj.transform.Find("PathfindingTarget");
        spawnedEnemy.GetComponent<AIPath>().maxSpeed = enemySpeed;
        currentActiveEnemies++;
        enemiesSpawnedThisWave++;
    }

    private float getSpawnTime() {
        float minTime = minSpawnCooldownSeconds * spawnMultiplier;
        float maxTime = maxSpawnCooldownSeconds * spawnMultiplier;
        float spawnTime = Random.Range(minTime, maxTime);
        return spawnTime;
    }

    private IEnumerator waitBeforeNextWave() {
        isWaitingForWaveStart = true;
        yield return new WaitForSeconds(waveCooldownSeconds1);
        if (!startOfGame) {
            currentWave++;
        }
        enemiesSpawnedThisWave = 0;
        waveNumberEvent.Raise(currentWave+1);
        Debug.Log("Wave #" + currentWave+1);
        yield return new WaitForSeconds(waveCooldownSeconds2);
        isWaitingForWaveStart = false;
        startOfGame = false;
    }

    public void e_EnemyDestroyed() {
        currentActiveEnemies--;
        score.Value += scoreIncrementAmount;
        audioSource.clip = killEnemySFX;
        audioSource.Play();
    }

    void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, spawnCircleRadius);
    }
}
