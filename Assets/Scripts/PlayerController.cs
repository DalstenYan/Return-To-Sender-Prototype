using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IPortalTravel
{

    [SerializeField]
    private float portalReachDistance = 10f;

    [SerializeField]
    [Tooltip("Array of all possible portal prefab types")]
    private GameObject[] portalPrefabs;
    [SerializeField]
    [Tooltip("Array of existing portals in the scene")]
    private GameObject[] existingPortals;

    [SerializeField]
    private LayerMask groundLayerMask;

    [SerializeField]
    private Bullet _storedPortalBullet;

    public bool IsTraveling { get; set; }

    private void Start()
    {
        existingPortals = new GameObject[portalPrefabs.Length];
    }

    public void OnPortal1() 
    {
        PlacePortalDown(0);
    }

    public void OnPortal2()
    {
        PlacePortalDown(1);
    }

    /// <summary>
    /// Places portals down depending on their prefabs
    /// </summary>
    /// <param name="portalIndex"></param>
    private void PlacePortalDown(int portalIndex) 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, portalReachDistance, groundLayerMask))
        {
            PortalControl(portalIndex);
            hit.point += Vector3.up;
            existingPortals[portalIndex] = Instantiate(portalPrefabs[portalIndex], hit.point, transform.rotation);

            Debug.Log("Placed " + portalPrefabs[portalIndex].name + " at: " + hit.point);

        }
    }

    /// <summary>
    /// Destroys existing portals if they exist
    /// </summary>
    /// <param name="portalIndex"></param>
    private void PortalControl(int portalIndex) 
    {
        if (existingPortals[portalIndex] == null)
            return;
        Destroy(existingPortals[portalIndex]);
    }

    public void SetStoredBullet(Bullet bullet) 
    {
        if(bullet != null)
            bullet.gameObject.SetActive(false);
        _storedPortalBullet = bullet;
    }

    public bool HasStoredBullet() 
    {
        return _storedPortalBullet != null;
    }

    public Bullet GetStoredBullet() 
    {
        _storedPortalBullet.gameObject.SetActive(true);
        return _storedPortalBullet;
    }

    void OnExit()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);

    }
}
