using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> enemyPrefabTypes;

    Dictionary<string, Queue<EnemyController>> enemyObjectPools;

    public static EnemyManager instance;
    [SerializeField]
    private bool organizeAllSpawners;
    [SerializeField]
    int totalEnemySpawn, existingEnemies, defeatedEnemies;

    //Enqueue existing enemy

    private void Start()
    {
        enemyObjectPools = new Dictionary<string, Queue<EnemyController>>();
        instance = this;
        foreach (GameObject obj in enemyPrefabTypes) 
        {
            enemyObjectPools.Add(obj.name, new Queue<EnemyController>());
        }

        if (!organizeAllSpawners)
            return;

        foreach (var obj in FindObjectsByType<SpawnerController>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)) 
        {
            obj.transform.parent = transform;
        }
    }

    public void EnqueueEnemy(string enemyType, EnemyController enemy) 
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);
        enemyObjectPools[enemyType].Enqueue(enemy);
        //Debug.Log("Added: " + enemy + ", pool is now " + enemyObjectPools[enemyType] + "with count: " + enemyObjectPools[enemyType].Count);
        defeatedEnemies++;
        if (defeatedEnemies >= totalEnemySpawn)
            GameManager.gm.gameWonEvent.Invoke();
    }

    /// <summary>
    /// Enqueue a NEW instantiated enemy
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    EnemyController CreateNewEnemy(string enemyType) 
    {
        var newEnemy = Instantiate(enemyPrefabTypes.Find(x => x.name == enemyType), Vector3.zero, Quaternion.identity).GetComponent<EnemyController>();
        return newEnemy;
    }

    public EnemyController DequeueEnemy(string enemyType)
    {
        existingEnemies++;
        //Debug.Log("Trying Dequeue... Current count is: " + enemyObjectPools[enemyType].Count);
        if (enemyObjectPools[enemyType].TryDequeue(out EnemyController enemyOut) && enemyOut != null) 
        {
            Debug.Log("Re-entered: " + enemyOut);
            return enemyOut;
        }
        return CreateNewEnemy(enemyType);
    }

    public bool HasMaxEnemiesSpawned() { return existingEnemies >= totalEnemySpawn;  }


    [ContextMenu("Activate Enemy Again")]
    public void ReactivateEnemy() 
    {
        var enemy = DequeueEnemy("basicEnemy");
        enemy.Activation(transform);

    }
}
