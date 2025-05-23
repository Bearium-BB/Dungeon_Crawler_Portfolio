using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public EntityStats stats;
    public PlayerInput playerInput;
    public Camera cam;
    public ScriptableObjectTransform pos;

    private Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    private bool groundedPlayer;

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerVelocity += transform.forward * 10;
            characterController.Move(playerVelocity);
        }
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector2 input = playerInput.actions.FindActionMap("Player").FindAction("Move").ReadValue<Vector2>();
        playerVelocity = new Vector3(input.x * stats.speed, playerVelocity.y, input.y * stats.speed);


        if (!playerInput.actions.FindActionMap("Player").FindAction("Move").inProgress)
        {
            //float currentVelocityX = playerVelocity.x;
            //float currentVelocityZ = playerVelocity.z;
            //float time = 0.1f;

            //float stopVelocityX = currentVelocityX / time;
            //float stopVelocityZ = currentVelocityZ / time;

            //playerVelocity -= new Vector3(stopVelocityX, 0, stopVelocityZ) * Time.deltaTime;
            playerVelocity = new Vector3(0, playerVelocity.y, 0);
        }

        groundedPlayer = characterController.isGrounded;

        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else if (playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        characterController.Move(playerVelocity * Time.deltaTime);

        if (playerInput.currentControlScheme != "Keyboard And Mouse")
        {
            if (playerInput.actions.FindActionMap("Player").FindAction("Move").inProgress)
            {
                Quaternion quaternion = Quaternion.LookRotation(playerVelocity);
                transform.rotation = new Quaternion(transform.rotation.x, quaternion.y, transform.rotation.z, quaternion.w);
            }
        }
        else
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = cam.nearClipPlane;

            Ray ray = cam.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray,out RaycastHit raycastHit))
            {
                Quaternion quaternion = Quaternion.LookRotation(raycastHit.point - transform.position);
                transform.rotation = new Quaternion(transform.rotation.x, quaternion.y, transform.rotation.z, quaternion.w);
            }
        }
        pos.position = transform.position;
        pos.rotation = transform.rotation;
    }

    private void OnEnable()
    {
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("Player").FindAction("Dodge").performed += Dodge;
    }
    private void OnDisable()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("Player").FindAction("Dodge").performed -= Dodge;
    }
}



