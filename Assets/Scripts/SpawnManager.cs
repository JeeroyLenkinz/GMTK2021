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
    public int maxAllowableEnemies;
    public float minSpawnCooldown;
    public float maxSpawnCooldown;
    public float minEnemySpeed;
    public float maxEnemySpeed;
    public float spawnCircleRadius;

    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject playerObj;
    [SerializeField]
    private IntReference score;
    public int scoreIncrementAmount;
    
    // Start is called before the first frame update
    void Awake()
    {
        spawnMultiplier = 1;
        timeUntilSpawn = minSpawnCooldown;
        score.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0 && currentActiveEnemies < maxAllowableEnemies) {
            spawnEnemy();
            timeUntilSpawn = getSpawnTime();
        }
    }

    private void spawnEnemy() {
        float enemySpeed = Random.Range(minEnemySpeed, maxEnemySpeed);
        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnCircleRadius;
        GameObject spawnedEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        spawnedEnemy.GetComponent<AIDestinationSetter>().target = playerObj.transform.Find("PathfindingTarget");
        spawnedEnemy.GetComponent<AIPath>().maxSpeed = enemySpeed;
        currentActiveEnemies++;
    }

    private float getSpawnTime() {
        float minTime = minSpawnCooldown * spawnMultiplier;
        float maxTime = maxSpawnCooldown * spawnMultiplier;
        float spawnTime = Random.Range(minTime, maxTime);
        return spawnTime;
    }

    public void e_EnemyDestroyed() {
        currentActiveEnemies--;
        score.Value += scoreIncrementAmount;
    }
}
