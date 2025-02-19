using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour, IPortalTravel
{
    private BulletManager bulletManager;
    [SerializeField] private bool hitAllies = false;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private bool inTransport = false;
    [SerializeField] private string sentPortal;

    [SerializeField]
    private Material regularBulletMaterial, allyHitBulletMaterial;

    public bool IsTraveling { get; set; }

    [SerializeField] private float timeBeforeDespawn = 6.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
    }
    private void OnEnable()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;

        StartCoroutine(DespawnTimer());
    }
    private void OnDisable()
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;

        StopCoroutine(DespawnTimer());  
    }

    private void Update()
    {
        if (enabled)
        {
            GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hitObject = other.gameObject;

        //When it hits an enemy, and it can hit allies
        if (hitObject.CompareTag("Enemy"))
        {
            if(hitAllies)
            {
                hitObject.GetComponent<EnemyController>().EnemyDeath();
                bulletManager.SendBullet(gameObject);
            }
            else
            {
                return;
            }
        }
        if (hitObject.CompareTag("Walls") || hitObject.CompareTag("Ground"))
        {
            bulletManager.SendBullet(gameObject);
        }
    }


    public void SetHitAllies(bool canHitAllies, string portal = "") 
    {
        sentPortal = portal;
        GetComponent<MeshRenderer>().material = canHitAllies ? allyHitBulletMaterial : regularBulletMaterial;
        hitAllies = canHitAllies;
    }

    private void OnDrawGizmos()
    {
        //ray for bullet direction (scene view only)
        Gizmos.DrawRay(transform.position, transform.forward * 1f);
    }

    //destroys bullet after serialized time duration
    public IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(timeBeforeDespawn);
        bulletManager.SendBullet(gameObject);
    }
}
