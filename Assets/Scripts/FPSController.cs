using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private float _walkSpeed = 6f;
    [SerializeField] private float _jumpPower = 7f;
    [SerializeField] private float _gravity = 10f;

    [SerializeField] private float _lookSpeed = 2f;
    [SerializeField] private float _lookXLimit = 45f;

    private bool canMove = true;
    private float curSpeedX, curSpeedY;
    private float mouseX, mouseY;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    CharacterController characterController;

    /// <summary>
    ///     Basic setup for scene
    /// </summary>
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    ///     used for movement and rotation
    /// </summary>
    void Update()
    {

        #region Handles Movement

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedY) + (right * curSpeedX);

        moveDirection.y = movementDirectionY;

        characterController.Move(moveDirection * Time.deltaTime);

        #endregion

        #region Handles Rotation

        //Rotate camera and self by mouse movement
        if (canMove)
        {
            rotationX += -mouseY * _lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -_lookXLimit, _lookXLimit);
            _playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX * _lookSpeed, 0);
        }

        #endregion

        // Player gravity when in the air
        if (!characterController.isGrounded)
        {
            moveDirection.y -= _gravity * Time.deltaTime;
        }
    }

    /// <summary>
    ///     Called implicitly when PlayerInput Move is used
    ///     Sets move speed to the player's input movement so that it can be modified by the camera rotation
    /// </summary>
    /// <param name="inputVector2"> movement input </param>
    public void OnMove(InputValue inputVector2)
    {
        curSpeedX = _walkSpeed * inputVector2.Get<Vector2>().x;
        curSpeedY = _walkSpeed * inputVector2.Get<Vector2>().y;
    }

    /// <summary>
    ///     Called implicitly when PlayerInput Jump is used
    ///     If grounded, sets player y move direction to jump power
    /// </summary>
    public void OnJump()
    {
        if (characterController.isGrounded)
        {
            moveDirection.y = _jumpPower;
        }
    }

    /// <summary>
    ///     Called implicitly when PlayerInput Look is used
    ///     Changes mouse x and y variables so that they can be used to calculate move direction
    /// </summary>
    /// <param name="mouseInput"> mouse input </param>
    public void OnLook(InputValue mouseInput)
    {
        mouseX = mouseInput.Get<Vector2>().x;
        mouseY = mouseInput.Get<Vector2>().y;
    }
}
