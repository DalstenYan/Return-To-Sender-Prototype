using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject _storedProjectile;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(CompareTag("PortalB"))
            return;
        Debug.Log("Entered " + gameObject.name + ":" + other.gameObject.name);

        GameObject otherPortal = GameObject.FindGameObjectWithTag("PortalB");

        if (otherPortal == null) 
        {
            if (other.gameObject.CompareTag("Bullet")) 
            {
                _storedProjectile = other.gameObject;
            }
            return;
        }

        //Transform otherPortalTransform = .transform;
        other.gameObject.transform.position = otherPortal.transform.position;
    }
}
