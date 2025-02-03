using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private bool hitAllies = false;
    [SerializeField] private float bulletSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }
    private void OnEnable()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.forward * bulletSpeed;
    }
    private void OnDisable()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject hitObject = collision.gameObject;

        if (hitObject.CompareTag("Enemy") && hitAllies) 
        {
            hitObject.GetComponent<EnemyController>().EnemyDeath();
        }
        bulletManager.SendBullet(gameObject);
    }

    public void FlipHitTarget() 
    {
        hitAllies = !hitAllies;
    }
}
