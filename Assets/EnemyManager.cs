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

    //Enqueue existing enemy

    private void Start()
    {
        enemyObjectPools = new Dictionary<string, Queue<EnemyController>>();
        instance = this;
    }

    public void EnqueueEnemy(string enemyType, EnemyController enemy) 
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(transform);
        if (!enemyObjectPools.TryGetValue(enemyType, out Queue<EnemyController> enemyPool))
        {
            enemyObjectPools.Add(enemyType, new Queue<EnemyController>());
        }
        enemyObjectPools[enemyType].Enqueue(enemy);
        Debug.Log("Added: " + enemy + ", pool is now " + enemyObjectPools[enemyType]);
    }

    /// <summary>
    /// Enqueue a NEW instantiated enemy
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    EnemyController EnqueueNewEnemy(string enemyType) 
    {
        var newEnemy = Instantiate(enemyPrefabTypes.Find(x => x.name == enemyType), Vector3.zero, Quaternion.identity).GetComponent<EnemyController>();
        enemyObjectPools[enemyType].Enqueue(newEnemy);
        return newEnemy;
    }

    public EnemyController DequeueEnemy(string enemyType)
    {
        if (enemyObjectPools[enemyType].TryDequeue(out EnemyController enemyOut)) 
        {
            return enemyOut;
        }
        return EnqueueNewEnemy(enemyType);
    }

    [ContextMenu("Activate Enemy Again")]
    public void ReactivateEnemy() 
    {
        var enemy = DequeueEnemy("basicEnemy");
        enemy.Activation();

    }
}
