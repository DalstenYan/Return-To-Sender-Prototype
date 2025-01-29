using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    private Stack bulletPool = new Stack();

    /// <summary>
    ///     Should be called when an enemy shoots
    /// 
    ///     Gets bullet from pool; if pool is empty instantiates new bullet for use
    /// </summary>
    /// <returns> Bullet from pool </returns>
    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            GameObject newBullet = Instantiate(bullet);
            return newBullet;
        }
        else
        {
            GameObject newBullet = (GameObject)bulletPool.Pop();
            newBullet.transform.parent = null;
            return newBullet;
        }
    }

    /// <summary>
    ///     Puts bullet into pool and sets as inactive
    /// </summary>
    /// <param name="b"> bullet to be pooled </param>
    public void SendBullet(GameObject b)
    {
        b.SetActive(false);
        b.transform.SetParent(transform);
        bulletPool.Push(b);
    }
}
