using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab1, enemyPrefab2;

    [SerializeField] private List<Transform> spawnPoints;

    [SerializeField] private Transform specialSpawn;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            SpawnEnemy(0);
        }
        else if (Input.GetKeyDown(KeyCode.F12))
        {
            SpawnEnemy(1);
        }
    }

    public void SpawnEnemy(int number)
    {
        int rand = Random.Range(0, spawnPoints.Count);

        GameObject enemy = null;

        if (number == 0)
        {
            enemy = Instantiate(enemyPrefab1);
            enemy.transform.position = spawnPoints[rand].position;
        }
        else if (number == 1)
        {
            enemy = Instantiate(enemyPrefab2);
            enemy.transform.position = specialSpawn.position;
        }
        
    }
}
