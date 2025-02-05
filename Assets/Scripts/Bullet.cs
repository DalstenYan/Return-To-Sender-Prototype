using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletManager bulletManager;
    [SerializeField] private bool hitAllies = false;
    [SerializeField] private float bulletSpeed;

    [SerializeField]
    private Material regularBulletMaterial, allyHitBulletMaterial;

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

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        //When it hits an enemy, and it can hit allies
        if (hitObject.CompareTag("Enemy") && hitAllies)
        {
            hitObject.GetComponent<EnemyController>().EnemyDeath();
        }
        else if(!hitObject.CompareTag("Walls"))
        {
            return;
        }

        bulletManager.SendBullet(gameObject);
    }


    public void SetHitAllies(bool canHitAllies) 
    {
        GetComponent<MeshRenderer>().material = canHitAllies ? allyHitBulletMaterial : regularBulletMaterial;
        hitAllies = canHitAllies;
    }
}
