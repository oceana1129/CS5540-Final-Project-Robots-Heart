using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MiloBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    public float followSpeed = 2.0f;
    public float rotationSpeed = 5.0f;
    public float stopDistance = 1.5f;
    public float rollSpeed = 4.0f;

    [Header("References")]
    public DialogueManager dialogueManager;
    public PauseMenuBehavior pauseMenuBehavior;
    public Transform playerTransform;
    public LevelManager levelManager;

    private bool isFollowing = false;
    private bool hasTriggeredWinMenu = false;
    private Animator anim;
    private CharacterController characterController;
    private PlayerMovement playerMovement;
    private Vector3 currentMovement;
    

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueChainComplete += HandleDialogueChainComplete;
        }
    }

    void Update()
    {
        if (anim.GetBool("Open_Anim") && !isFollowing)
        {
            StartCoroutine(StartFollowingAfterDelay(3.0f));
        }

        if (isFollowing && playerTransform != null)
        {
            FollowPlayer();
            HandleRotation();
            HandleAnimation();
        }
    }

    private IEnumerator StartFollowingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFollowing = true;
        if (levelManager != null)
        {
            levelManager.MiloActivated();
        }
    }

    private void FollowPlayer()
    {
        if (playerTransform == null)
        {
            return;
        }
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stopDistance)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            currentMovement = direction * (anim.GetBool("Roll_Anim") ? rollSpeed : followSpeed);
            characterController.Move(currentMovement * Time.deltaTime);
        }
        else
        {
            currentMovement = Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        if (currentMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(currentMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleAnimation()
    {
        // Walk
        if (playerMovement.IsMovementPressed)
        {
            anim.SetBool("Walk_Anim", true);
        }
        else
        {
            anim.SetBool("Walk_Anim", false);
        }

        // Roll
        if (playerMovement.IsRunPressed)
        {
            anim.SetBool("Roll_Anim", true);
        }
        else
        {
            anim.SetBool("Roll_Anim", false);
        }
    }
    private void HandleDialogueChainComplete()
    {
        if (!hasTriggeredWinMenu)
        {
            hasTriggeredWinMenu = true;
            StartCoroutine(TriggerWinMenuWithDelay());
        }
    }

    private IEnumerator TriggerWinMenuWithDelay()
    {
        yield return new WaitForSeconds(0.3f);

        // Trigger the win menu after delay
        if (pauseMenuBehavior != null)
        {
            pauseMenuBehavior.ViewWinMenu();
        }
        else
        {
            Debug.LogWarning("PauseMenuBehavior reference is missing!");
        }
    }

    private void OnDestroy()
    {
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueChainComplete -= HandleDialogueChainComplete;
        }
    }
}
