using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private BulletManager bulletManager;

    [Header("Shooting Variables")][Space(5)]

    [Tooltip("Bullet distance per second")][Min (0)]
    [SerializeField] private float bulletSpeed;

    [Tooltip("Instantiation position of bullets")]
    [SerializeField] private Transform shootPosition;

    [Tooltip("Delay between enemy firing bullets")][Min (0)]
    [SerializeField] private float shootDelay = 0.5f;

    [Space (5)]

    [Header ("Rotation Variables")][Space (5)]

    [Tooltip ("If enemy should track player")]
    [SerializeField] private bool shouldTrackPlayer;

    [Tooltip("Rotation speed of enemy (degrees per second) ((i think))")] [Min (0)] 
    [SerializeField] private float lookSpeed = 30;

    [Tooltip ("Distance that enemy will begin to track the player")] [Min(0)]
    [SerializeField] private float lookRange = 30;

    private GameObject player;

    void Start()
    {
        Activation(transform);
        player = GameObject.FindWithTag("Player"); print("Enemy detected player:" + player.name);
    }

    private void Update()
    {
        //check shouldTrack bool & if within range
        if (shouldTrackPlayer && Vector3.Distance(player.transform.position, transform.position) <= lookRange)
        {
            //direction vector
            Vector3 relativePos = player.transform.position - transform.position;
            //create new rotation quaternion
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //rotate towards quaternion with lookspeed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * lookSpeed);
        }
    }

    void OnEnable()
    {
        bulletManager = GameObject.Find("BulletManager").GetComponent<BulletManager>();
        StartCoroutine(ShootLoop());
    }

    /// <summary>
    ///     Gets bullet from pool and shoots towards transform.forward with bulletSpeed variable
    /// </summary>
    void Shoot()
    {
        //get bullet from bullet pool
        GameObject bullet = bulletManager.GetBullet();

        //set bullet position/rotation
        bullet.transform.position = shootPosition.position;
        bullet.transform.rotation = transform.rotation;

        //set bullet velocity
        bullet.GetComponent<Rigidbody>().linearVelocity = transform.forward * bulletSpeed;
    }

    IEnumerator ShootLoop() //shoot on a timer with shootDelay
    {
        yield return new WaitForSeconds(shootDelay);
        Shoot();
        StartCoroutine(ShootLoop());
    }

    public void Activation(Transform location) 
    {
        transform.position = location.position;
        transform.parent = location;
        gameObject.SetActive(true);
    }

    
    [ContextMenu("Simulate Death")]
    public void EnemyDeath() 
    {
        StopAllCoroutines();
        Debug.Log(EnemyManager.instance);
        EnemyManager.instance.EnqueueEnemy("basicEnemy", this);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(shootPosition.position, transform.forward * 2f);
        //draws ray in shoot direction for visible sightline (editor only)
    }
}
