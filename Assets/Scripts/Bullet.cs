using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletManager bulletManager;
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
        if(!collision.gameObject.CompareTag("Enemy"))
            bulletManager.SendBullet(gameObject);
    }
}
