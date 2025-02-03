using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (CompareTag("PortalB") && player.HasStoredBullet())
        {
            player.GetStoredBullet().transform.position = transform.position;
            player.SetStoredBullet(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CompareTag("PortalB"))
            return;
        Debug.Log("Entered " + gameObject.name + ":" + other.gameObject.name);

        GameObject otherPortal = GameObject.FindGameObjectWithTag("PortalB");

        //When the collision is not a bullet, and the other portal is inactive
        if (!other.gameObject.CompareTag("Bullet") && otherPortal == null)
        {
            //Don't proceed further or teleport 
            return;
        }

        //When the collision is a bullet
        if (other.gameObject.CompareTag("Bullet")) 
        {
            //Get the bullet, and enable it to hit enemies
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            bullet.FlipHitTarget();

            //If the other portal is inactive, store the bullet if there is none, don't teleport
            if (otherPortal == null && !player.HasStoredBullet())
            {
                player.SetStoredBullet(bullet);
                return;
            }
        }

        if (otherPortal == null)
            return;
        
        other.gameObject.transform.position = otherPortal.transform.position;
    }
    
}
