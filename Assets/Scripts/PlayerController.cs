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

    private Vector3 initialRotation;
    private bool rotationInProgress = false;
    private float xDiff = 0f;
    private float zDiff = 0f;
    private int rotationCount = 0;

    private void Start()
    {
        _maxLives = _playerLives;
        existingPortals = new GameObject[portalPrefabs.Length];
    }

    void Update()
    {
        if(rotationInProgress ==  true)
        {
            transform.Rotate(xDiff, 0, zDiff);
            rotationCount++;
            if (rotationCount >= 200)
            {
                rotationInProgress = false;
                rotationCount = 0;
                xDiff = 0f;
                zDiff = 0f;
                transform.Rotate(transform.eulerAngles.x * -1, 0, transform.eulerAngles.z * -1);
            }
            Debug.Log("The player character was rotated!");
        }
        else if(transform.eulerAngles.x != 0 || transform.eulerAngles.z != 0)
        {
            rotationInProgress = true;
            if (transform.eulerAngles.x < 180)
            {
                xDiff = transform.eulerAngles.x / -200;
            }
            else
            {
                xDiff = (360 - transform.eulerAngles.x) / 200;
            }
            if (transform.eulerAngles.z < 180)
            {
                zDiff = transform.eulerAngles.z / -200;
            }
            else
            {
                zDiff = (360 - transform.eulerAngles.z) / 200;
            }
        }
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
        if (Time.timeScale == 0)
            return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int wallMask = LayerMask.GetMask("Walls");
        int groundMask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out RaycastHit hit, portalReachDistance, wallMask))
        {
            PortalControl(portalIndex);
            existingPortals[portalIndex] = Instantiate(portalPrefabs[portalIndex], hit.point, hit.transform.rotation);
            existingPortals[portalIndex].transform.Rotate(0, 90, 0);
            existingPortals[portalIndex].transform.Translate(ray.direction * 0.15f);

            Debug.Log("Placed " + portalPrefabs[portalIndex].name + " at: " + hit.point);

        }
        else if(Physics.Raycast(ray, out RaycastHit hit2, portalReachDistance, groundMask))
        {
            PortalControl(portalIndex);
            existingPortals[portalIndex] = Instantiate(portalPrefabs[portalIndex], hit2.point, hit2.transform.rotation);
            existingPortals[portalIndex].transform.Rotate(90, 0, 0);
            existingPortals[portalIndex].transform.Translate(Vector3.forward * -0.25f);

            Debug.Log("Placed " + portalPrefabs[portalIndex].name + " at: " + hit2.point);
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
