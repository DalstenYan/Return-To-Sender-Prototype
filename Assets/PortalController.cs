using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(CompareTag("PortalB"))
            return;
        Transform otherPortalTransform = GameObject.FindGameObjectWithTag("PortalB").transform;
        other.gameObject.transform.position = otherPortalTransform.position;
    }

    private void Update()
    {
        //player.transform.position = new Vector3(2, 2, 2);
    }
}
