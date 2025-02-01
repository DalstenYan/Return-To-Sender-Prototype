using NUnit.Framework;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_spawnLocations .Count == 0)
            _spawnLocations = GetComponentsInChildren<Transform>().ToList<Transform>();
        StartCoroutine(SpawnAnEnemy());
    }

    IEnumerator SpawnAnEnemy() 
    {
        yield return new WaitForSeconds(_spawnInterval);
        var enemy = EnemyManager.instance.DequeueEnemy(GetEnemyNameToSpawn());
        Transform spawnLocation = _spawnLocations[Random.Range(0, _spawnLocations.Count)];
        enemy.Activation(spawnLocation.position);

        StartCoroutine(SpawnAnEnemy());
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
}
