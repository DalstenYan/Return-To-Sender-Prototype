using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform cameraMain;

    [SerializeField]
    private InputActionReference movement, jump, look;

    [Header("Player Variables")]
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;
    [SerializeField]
    private float distance = 10f;

    [SerializeField]
    [Tooltip("Array of all possible portal prefab types")]
    private GameObject[] portalPrefabs;
    [SerializeField]
    [Tooltip("Array of existing portals in the scene")]
    private GameObject[] existingPortals;

    [SerializeField]
    private LayerMask groundLayerMask;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    private void OnEnable()
    {
        movement.action.Enable();
        jump.action.Enable();
    }

    private void OnDisable()
    {
        movement.action.Disable();
        jump.action.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = gameObject.GetComponent<CharacterController>();
        existingPortals = new GameObject[portalPrefabs.Length];
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 keyInputs = movement.action.ReadValue<Vector2>();
        Vector3 move = new(keyInputs.x, 0f, keyInputs.y);
        move = cameraMain.transform.forward * move.z + cameraMain.transform.right * move.x;
        move.y = 0f;
        controller.Move(playerSpeed * Time.deltaTime * move);

        // Makes the player jump
        if (jump.action.IsInProgress() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        if (keyInputs != Vector2.zero) 
        {
            float targetAngle = Mathf.Atan2(keyInputs.x, keyInputs.y) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void PlacePortalOne(InputAction.CallbackContext context) 
    {
        if (!context.performed)
            return;
        PlacePortalDown(0);
        
    }

    public void PlacePortalTwo(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        PlacePortalDown(1);

    }

    /// <summary>
    /// Places portals down depending on their prefabs
    /// </summary>
    /// <param name="portalIndex"></param>
    private void PlacePortalDown(int portalIndex) 
    {
        Ray ray = Camera.main.ScreenPointToRay(look.action.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit, distance, groundLayerMask))
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

}
