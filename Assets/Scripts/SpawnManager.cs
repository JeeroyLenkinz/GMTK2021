using ScriptableObjectArchitecture;
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
    public float waveCooldownSeconds;
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
    
    // Start is called before the first frame update
    void Awake()
    {
        spawnMultiplier = 1;
        timeUntilSpawn = minSpawnCooldownSeconds;
        score.Value = 0;
        currentWave = 0;
        hasWon = false;
        isWaitingForWaveStart = false;
        enemiesSpawnedThisWave = 0;
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
            StartCoroutine(waitBeforeNextWave());
            if (currentWave < (enemiesPerWave.Count - 1)) {
                currentWave++;
                enemiesSpawnedThisWave = 0;
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
        yield return new WaitForSeconds(waveCooldownSeconds);
        isWaitingForWaveStart = false;
    }

    public void e_EnemyDestroyed() {
        currentActiveEnemies--;
        score.Value += scoreIncrementAmount;
    }
}
