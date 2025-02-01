using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform shootPosition;

    void Start()
    {
        Activation(transform.position);
    }

    private void OnEnable()
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
        bullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
    }

    public IEnumerator ShootLoop()
    {
        Shoot();
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(ShootLoop());
    }

    public void Activation(Vector3 location) 
    {
        transform.position = location;
        gameObject.SetActive(true);
    }
    
    [ContextMenu("Simulate Death")]
    void EnemyDeath() 
    {
        StopAllCoroutines();
        Debug.Log(EnemyManager.instance);
        EnemyManager.instance.EnqueueEnemy("basicEnemy", this);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(shootPosition.position, -transform.forward * 0.5f);
    }
}
