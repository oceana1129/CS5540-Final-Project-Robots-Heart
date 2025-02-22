using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    private bool isAimPressed;
    private bool isShooting;

    [Header("Shooting Settings")]
    private float nextFireTime = 0f;        // time tracking for cooldown
    private float fireRate = 0.5f;          // seconds between shots
    private PlayerInput playerInput;        // player input reference

    [SerializeField] private Transform firePoint;               // where the projectile is instantiated
    [SerializeField] private GameObject projectilePrefab;       // projectile to shoot
    [SerializeField] private Light aimLight;                    // aim light for shooting indication
    [SerializeField] private Camera mainCamera;                 // main camera refence
    [SerializeField] private PlayerMovement playerMovement;     // player movement reference

    void Awake()
    {
        playerInput = new PlayerInput();                            // get player input

        if (!playerMovement)                                        // player movement not added
            playerMovement = GetComponent<PlayerMovement>();            // get reference

        if (!mainCamera)                                            // main camera not added
            mainCamera = Camera.main;                                   // get reference

        playerInput.Enable();                                       // enable player inputs
        
        // aim inputs
        playerInput.Controls.Aiming.started += OnAimInput;
        playerInput.Controls.Aiming.canceled += OnAimInput;
        
        // shoot inputs
        playerInput.Controls.Shoot.started += OnShootInput;
        playerInput.Controls.Shoot.canceled += OnShootInput;
    }

    void Update()
    {
        PlayerMovement.IsAiming = isAimPressed;                 // set movement variable for aim pressed
        PlayerMovement.CanRotate = !isAimPressed;               // rotation disabled when aiming

        // toggle aiming light
        if (aimLight != null)
            aimLight.enabled = isAimPressed;
    }

    void OnAimInput(InputAction.CallbackContext context) 
    {
        isAimPressed = context.ReadValueAsButton();
    }

    void OnShootInput(InputAction.CallbackContext context) 
    {
        isShooting = context.ReadValueAsButton();
        if (isAimPressed && Time.time >= nextFireTime)  // can shoot if cooldown is over
        {
            FireProjectile();                               // fire projectile
            nextFireTime = Time.time + fireRate;            // set shoot timer
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab != null && firePoint != null && isAimPressed)
        {

            Vector3 aimDirection = GetMouseWorldPosition() - firePoint.position;    // get mouse position
            aimDirection.y = 0f; // TODO vertical aiming set later
            firePoint.rotation = Quaternion.LookRotation(aimDirection); // rotate firePoint

            // spawn and shoot projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            if (rb)
            {
                rb.linearVelocity = firePoint.forward * 10f; // apply force
            }
        }
        else
        {
            Debug.LogWarning("Projectile or Fire Point not assigned!");
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());  // get mouse position

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point; // return raycast hit
        }
        return firePoint.position + firePoint.forward * 10f; // default forward direction
    }

    void OnEnable()
    {
        playerInput.Controls.Enable();
    }

    void OnDisable()
    {
        playerInput.Controls.Disable();
    }
}
