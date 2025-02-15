using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [SerializeField]
    private float _spawnInterval = 5;

    [SerializeField]
    [Tooltip("Press \"+\" to add an enemy type/name to spawn, at least one name is required. ")]
    private string[] _enemiesToSpawn;

    [SerializeField]
    [Tooltip("Press \"+\" to add a Transform location where the enemy COULD spawn. Ensure all transforms are created as children of the Spawner object. If no transforms are added, it will default to the Spawner's transform.")]
    private List<Transform> _spawnLocations;

    Dictionary<Transform, GameObject> _entitiesInScene;

    private List<Transform> _openLocations;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_spawnLocations.Count == 0)
            _spawnLocations = GetComponentsInChildren<Transform>().ToList<Transform>();
        StartCoroutine(SpawnEnemyCycle());
        _entitiesInScene = new();
        _openLocations = new();

        foreach (Transform t in _spawnLocations) 
        {
            _entitiesInScene[t] = null;
        }

    }

    IEnumerator SpawnEnemyCycle() 
    {
        yield return new WaitForSeconds(_spawnInterval);

        if (GetRemainingValidSpawnSpots() > 0) 
        {
            SpawnEnemy();
        }
        
        StartCoroutine(SpawnEnemyCycle());
    }

    void SpawnEnemy() 
    {
        int spawnIndex = Random.Range(0, _spawnLocations.Count);
        Transform spawnPosition = _spawnLocations[spawnIndex];

        //if the enemy already exists
        if (_entitiesInScene.TryGetValue(spawnPosition, out GameObject potentialEnemy) && potentialEnemy != null)
        {
            //Debug.Log("Enemy already exists here: " + spawnPosition.position);
            spawnPosition = _openLocations[Random.Range(0, _openLocations.Count)];

            //Debug.Log("Going to a new location: " + spawnPosition.position);
        }

        //if the enemy doesn't exist, + instantiating enemy
        //Debug.Log("Creating new enemy at: " + spawnPosition.position);
        var enemy = EnemyManager.instance.DequeueEnemy(GetEnemyNameToSpawn());
        _entitiesInScene[spawnPosition] = enemy.gameObject;
        enemy.Activation(spawnPosition);
        
    }


    string GetEnemyNameToSpawn() 
    {
        if (_enemiesToSpawn.Length == 0) 
        {
            Debug.LogWarning(gameObject.name + " has no enemy name types listed, please provide some enemy types!");
            Debug.Break();
        }
        return _enemiesToSpawn[Random.Range(0, _enemiesToSpawn.Length)];
    }

    int GetRemainingValidSpawnSpots() 
    {
        int count = 0;
        _openLocations.Clear();
        foreach (Transform transform in _entitiesInScene.Keys) 
        {
            if (_entitiesInScene[transform] == null || transform.childCount == 0) 
            {
                _openLocations.Add(transform);
                count++;
            }
        }
        return count;
    }
}
