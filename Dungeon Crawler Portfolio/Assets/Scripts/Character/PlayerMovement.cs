using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    public CharacterStats stats;
    public PlayerInput playerInput;
    public Camera cam;
    public ScriptableObjectTransform pos;
    public ScriptableObjectCharacterControls inputActions;

    private Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    private bool groundedPlayer;

    void Awake()
    {
        inputActions.value.Player.Enable();
        inputActions.value.Player.Dodge.performed += Dodge;

        pos.value = transform;
    }
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

        Vector2 input = inputActions.value.Player.Move.ReadValue<Vector2>();
        playerVelocity = new Vector3(input.x * stats.speed, playerVelocity.y, input.y * stats.speed);


        if (!inputActions.value.Player.Move.inProgress)
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
            if (inputActions.value.Player.Move.inProgress)
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
    }
}
