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
        GameObject newBullet;
        if (bulletPool.Count == 0)
        {
            newBullet = Instantiate(bullet);
        }
        else
        {
            newBullet = (GameObject)bulletPool.Pop();
        }
        newBullet.transform.parent = transform.GetChild(0);
        newBullet.SetActive(true);
        return newBullet;
    }

    /// <summary>
    ///     Puts bullet into pool and sets as inactive
    /// </summary>
    /// <param name="b"> bullet to be pooled </param>
    public void SendBullet(GameObject b)
    {
        b.GetComponent<Bullet>().SetHitAllies(false);
        b.SetActive(false);
        b.transform.SetParent(transform.GetChild(1));
        bulletPool.Push(b);
    }
}
