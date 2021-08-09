using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public int amountEnemies = 5;

    private void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < amountEnemies; i++)
        {
            Transform spawnPoint = GetRandomSpawnPoint();
            GameObject enemy = SpawnEnemy(spawnPoint);
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        var index = Random.Range(0, spawnPoints.Count);
        var result = spawnPoints[index];
        spawnPoints.RemoveRange(index, 1);
        return result;
    }

    private GameObject SpawnEnemy(Transform spawnPoint)
    {
        // Сюда можно дописать использование пула для врагов или более сложный алгоритм выбора противника
        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        return Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
