using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private Transform shootPosition;

    void Start()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();

        StartCoroutine(ShootLoop());
    }

    /// <summary>
    ///     Gets bullet from pool and shoots
    /// </summary>
    void Shoot()
    {
        GameObject bullet = bulletManager.GetBullet();
        bullet.transform.position = shootPosition.position;
        bullet.transform.rotation = transform.rotation;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().linearVelocity = Vector3.right * 5;
    }

    public IEnumerator ShootLoop()
    {
        Shoot();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootLoop());
    }
}
