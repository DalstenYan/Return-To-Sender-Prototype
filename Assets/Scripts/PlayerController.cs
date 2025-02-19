using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IPortalTravel
{
    [SerializeField]
    int _playerLives;

    int _maxLives;

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
    [SerializeField]
    private string _enemyBulletTag;

    public bool IsTraveling { get; set; }

    private void Start()
    {
        _maxLives = _playerLives;
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_enemyBulletTag))
        {
            if (_playerLives <= 0)
                return;
            Destroy(other.gameObject);
            LoseLives();
        }

        if (other.gameObject.CompareTag("HealthItem")) 
        {
            Destroy(other.transform.parent.gameObject);
            GainLives();
        }
    }

    [ContextMenu("Subtract Life")]
    public void LoseLives() 
    {
        _playerLives--;
        UIManager.Instance.UpdateLostLifeUI(_playerLives);
        if (_playerLives <= 0)
            GameManager.gm.GameOver();
    }

    [ContextMenu("Add Life")]
    public void GainLives()
    {
        if (_playerLives >= _maxLives)
            return;
        _playerLives++;
        UIManager.Instance.UpdateLostLifeUI(_playerLives);
    }

}
