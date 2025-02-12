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
            player.GetStoredBullet().transform.rotation = transform.rotation;
            player.SetStoredBullet(null);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered " + gameObject.name + ":" + other.gameObject.name);

        GameObject otherPortal = null;

        otherPortal = CompareTag("PortalB") ? GameObject.FindGameObjectWithTag("PortalA") : GameObject.FindGameObjectWithTag("PortalB");

        
        if (other.gameObject.TryGetComponent<IPortalTravel>(out var traveller) && traveller.IsTraveling) 
        {
            return;
        }

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

            bullet.SetHitAllies(true);

            //If the other portal is inactive, store the bullet if there is none, don't teleport
            if (otherPortal == null && !player.HasStoredBullet())
            {
                player.SetStoredBullet(bullet);
                return;
            }
        }

        if (otherPortal == null)
            return;
        
        // Old rotation, faces the bullets in the same direction as the new portal
        other.gameObject.transform.position = otherPortal.transform.position;
        //other.gameObject.transform.rotation = otherPortal.transform.rotation;  

        // New rotation, rotates the bullets based on the angle between the two portals
        Quaternion angle = Quaternion.FromToRotation(this.transform.forward, otherPortal.transform.forward);
        other.gameObject.transform.Rotate(angle.eulerAngles);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited " + name + ": " + other.gameObject);
        if (other.gameObject.TryGetComponent<IPortalTravel>(out var traveller))
        {
            traveller.FlipTraveling();
        }
    }

}
