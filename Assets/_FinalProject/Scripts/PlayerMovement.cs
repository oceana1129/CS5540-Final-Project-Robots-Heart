using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public bool canMove = false;
    public float speed = 1.0f;
    public float runSpeed = 3.0f;
    public float jumpHeight = 2.0f;
    public float gravity = 9.81f;
    public float rotationFactorPerFrame = 15.0f;
    public static bool IsAiming {get; set;} = false;
    public static bool CanRotate {get; set;} = true;

    [Header("SFX Settings")]
    public AudioClip jump1;
    public AudioClip jump2;

    // private variables
    private float scaleMultiplier = 1f;
    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private float verticalVelocity;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    private bool isJumping;

    [Header("References")]
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;
    private AudioSource audioSource;

    // Public getters for Milo animation
    public bool IsRunPressed => isRunPressed;
    public bool IsMovementPressed => isMovementPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        scaleMultiplier = transform.localScale.y;
        speed *= scaleMultiplier;
        runSpeed *= scaleMultiplier;
        jumpHeight *= scaleMultiplier;
        gravity *= scaleMultiplier;

        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Controls.Move.started += OnMovementInput;
        playerInput.Controls.Move.canceled += OnMovementInput;
        playerInput.Controls.Move.performed += OnMovementInput;

        playerInput.Controls.Run.started += OnRunInput;
        playerInput.Controls.Run.canceled += OnRunInput;

        playerInput.Controls.Jump.started += OnJumpInput;
        playerInput.Controls.Jump.canceled += OnJumpInput;
    }

    // Update is called once per frame     
    void Update()
    {
        ApplyGravity();

        if (CanRotate)
            HandleRotation();

        HandleAnimation();

        Vector3 movement = isRunPressed ? currentRunMovement : currentMovement;
        characterController.Move(movement * Time.deltaTime);
    }

    void OnMovementInput(InputAction.CallbackContext context) {
        currentMovementInput = context.ReadValue<Vector2>();

        // walking
        currentMovement.x = currentMovementInput.x * speed;
        currentMovement.z = currentMovementInput.y * speed;

        // running
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;

        // check if the player is moving
        isMovementPressed = currentMovementInput.sqrMagnitude > 0.01f;
    }

    void OnRunInput(InputAction.CallbackContext context) 
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnJumpInput(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
        AudioClip jumpingSfx = Random.Range(1, 3) == 1 ? jump1 : jump2;

        // Only jump if the player is grounded
        if (isJumpPressed && characterController.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(2 * jumpHeight * gravity); // apply jump force
            isJumping = true;

            if (jumpingSfx)
                audioSource.PlayOneShot(jumpingSfx);
            else
                Debug.LogError("No jumping sound effect available");
        }
    }

    void HandleRotation() 
    {
        Vector3 positionToLookAt = new Vector3(currentMovement.x, 0.0f, currentMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        
    }

    void ApplyGravity()
    {
        if (characterController.isGrounded) // if touching ground
        {
            if (isJumping)
            {
                isJumping = false; // reset jump when landed
            }
            else
            {
                verticalVelocity = -2f; // keep character grounded
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // apply gravity
        }

        // apply vertical movement to both walking and running
        currentMovement.y = verticalVelocity;
        currentRunMovement.y = verticalVelocity;
    }


    void HandleAnimation() 
    {
        float velocityX = currentMovementInput.x;
        float velocityY = currentMovementInput.y;

        animator.SetBool("isMoving", isMovementPressed);
        animator.SetBool("isRunning", isMovementPressed && isRunPressed);
        animator.SetBool("isGrounded", characterController.isGrounded);
        animator.SetBool("isJumping", isJumping);
        animator.SetFloat("velocityX", velocityX);
        animator.SetFloat("velocityY", velocityY);
        animator.SetBool("isAiming", IsAiming);
        
    }

    void OnEnable()
    {
        playerInput.Controls.Enable();
    }

    void OnDisable() {
        playerInput.Controls.Disable();
    }

    public void PauseMovement(bool pause)
    {
        Debug.Log("pausing movement " + !pause);
        canMove = !pause;

        // Optionally, you can freeze the player position by setting velocity to 0
        if (canMove)
        {
            OnEnable();
        }
        else
        {
            OnDisable();
        }
    }
}

