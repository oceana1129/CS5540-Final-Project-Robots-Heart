using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;
/// <summary>
/// Goes through all available dialogue in the dialogue manager
/// 
/// Should be able to do the following:
/// - Can read through a list of dialogue
/// - Can be triggered by interacting within trigger area (E)
/// - Can be automatically triggered by entering an area
/// - Can be made to trigger only once
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary> Represents a unit of dialogue by a single character </summary>
    [Serializable]
    public class Dialogue
    {
        /// <summary> Represents an single line of dialogue </summary>
        [Serializable]
        public class Line
        {
            [Header("Line settings")]
            public bool isSpokenOutloud = true;
            public Emotion emotion;
            public ScrollSpeed scrollSpeed;

            [Header("Dialogue text")]
            public string text;
        }

        [Header("Single Dialogue Settings")]
        public string characterName = "AURA";
        public Line[] lines;
        public enum Emotion { Neutral, Angry, Happy }
        public enum ScrollSpeed { Slow, Medium, Fast, ExtraFast };

    }

    [Header("Dialogue Trigger Settings")]
    public bool canTriggerDialogueChain = true;    // can this dialogue be triggered?
    public bool TriggerDialogueChainByEnteringArea = false;
    public bool TriggerDialogueChainByInteraction = false;
    public bool TriggerDialogueChainOnlyOnce = false;
    
    [Header("Dialogue Display UI Settings")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterNameText;
    public GameObject nextButton;

    [Header("All Dialogue")]
    /// <summary> The dialogue chain. Made up of Dialogue which is made up of Lines. </summary>
    public Dialogue[] dialogues; 
    
    // private variables

    [Header("Player Inputs")]
    private PlayerInput playerInput;    // to see if player has clicked an interact/next button
    private bool isInteractPressed = false; // if the interact button has been pressed

    [Header("Dialogue Bools")]
    private bool isPlayerInRadius = false; // is player in the dialogue
    private bool dialogueChainIsActive = false;  // register if dialogue is active
    private bool isTypingLine = false;

    [Header("Dialogue Timer")]
    private Coroutine dialogueCoroutine;

    [Header("Position in Dialogue Chain")]
    private int currentLineIndex = 0;
    private int currentDialogueIndex = 0;
    

    void Awake()
    {
        if (!dialogueBox || !dialogueText || !characterNameText || !nextButton)
            FindDialogueUIElements();
        
        if (!dialogueBox || !dialogueText || !characterNameText || !nextButton)
        {
            Debug.LogError("Dialogue box components are missing. Please add via the inspector");
            return;
        }

        SetUpPlayerInput();
    }
    
    /// <summary> Find dialogue UI elements so as to not manually add them each time </summary>
    void FindDialogueUIElements()
    {
        GameObject root = GameObject.FindGameObjectWithTag("DialogueUI");

        if (root != null)
        {
            dialogueBox = root.transform.Find("DialogueContainer")?.gameObject;
            dialogueText = dialogueBox.transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>();
            characterNameText = dialogueBox.transform.Find("DialogueName")?.GetComponent<TextMeshProUGUI>();
            nextButton = dialogueBox.transform.Find("DialogueNext")?.gameObject;

            if (!dialogueText || !characterNameText || !nextButton)
                Debug.LogWarning("Some Dialogue UI elements were not found as expected.");
        }
        else
        {
            Debug.LogError("Dialogue UI root with tag 'DialogueUI' not found.");
        }
    }

    /// <summary> Set up the interact player input from PlayerInput </summary>
    void SetUpPlayerInput() 
    {
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Controls.Examine.started += OnInteractInput;
        playerInput.Controls.Examine.canceled += OnInteractInput;
        playerInput.Controls.Examine.performed += OnInteractInput;
    }

    /// <summary> Set up the interact player input to appropriate booleon </summary>
    void OnInteractInput(InputAction.CallbackContext context) 
    {
        // isInteractPressed = context.ReadValueAsButton();
        if (context.phase == InputActionPhase.Performed)
        {
            isInteractPressed = true;
        }
    }

    void Update()
    {
        if (TriggerDialogueChainByInteraction && isPlayerInRadius && isInteractPressed && canTriggerDialogueChain && !dialogueChainIsActive)
        {
            Debug.Log("Triggered by pressing button inside dialogue zone");
            TriggerDialogueChain();
            isInteractPressed = false; // prevent retriggering
        }

        if (dialogueChainIsActive && isInteractPressed && !isTypingLine)
        {
            OnNextPressed();
            isInteractPressed = false; // prevent retriggering
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRadius = true;
        }

        if (TriggerDialogueChainByEnteringArea && canTriggerDialogueChain)
        {
            Debug.Log("Triggered by entering dialogue zone");
            TriggerDialogueChain();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerInRadius = false;
    }

    /// <summary> Start the dialogue chain </summary>
    void TriggerDialogueChain()
    {
        Debug.Log("Started dialogue chain");
        // to prevent any potential retriggers
        if (dialogueChainIsActive || !canTriggerDialogueChain)
            return;

        currentDialogueIndex = 0;
        dialogueChainIsActive = true;

        StartDialogue();
    }

    /// <summary> Start a new set of dialogue </summary>
    void StartDialogue()
    {
        Debug.Log("Started new dialogue index: " + currentDialogueIndex);

        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("No dialogue found.");
            return;
        }

        Dialogue currentDialogue = dialogues[currentDialogueIndex];    // get current dialogue

        if (currentDialogue.lines == null || currentDialogue.lines.Length == 0)
        {
            Debug.LogWarning("Dialogue has no lines.");
            EndDialogue();
            return;
        }

        AnimateDialogueBox(true);   // open up dialogue ui

        characterNameText.text = currentDialogue.characterName;    // set character name
        currentLineIndex = 0;   // start the line index in the dialogue

        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(TypeLine(currentDialogue.lines[currentLineIndex]));
    }

    /// <summary> Type a line of dialogue </summary>
    IEnumerator TypeLine(Dialogue.Line line)
    {   
        Debug.Log("Started new line index: " + currentLineIndex);
        // start typing line
        isTypingLine = true;
        AnimateNextButton(false); // hide next button
        dialogueText.text = ""; // set dialogue text to empty first
        float delay = GetScrollSpeedDelay(line.scrollSpeed);    // get the scroll speed

        // add a letter to the dialogue text based on scroll speed
        foreach (char letter in line.text.ToCharArray())
        {
            // Debug.Log("writing letter " + letter);
            dialogueText.text += letter;
            yield return new WaitForSeconds(delay);
        }

        // no longer typing
        isTypingLine = false;
        AnimateNextButton(true); // show "Next" button once line is done
    }

    /// <summary> Player triggers next line or dialogue </summary>
    public void OnNextPressed()
    {
        Debug.Log("On next pressed");
        // later add a skip function
        // for now don't do anything
        if (isTypingLine) return;

        // indicates start next line
        currentLineIndex++;

        // if there are more lines left in dialogue, type a line
        if (currentLineIndex < dialogues[currentDialogueIndex].lines.Length)
        {
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            AnimateNextButton(false);
            dialogueCoroutine = StartCoroutine(TypeLine(dialogues[currentDialogueIndex].lines[currentLineIndex]));
        }
        // otherwise, dialogue box is considered completed
        else
        {
            EndDialogue();
        }
    }

    /// <summary> End the dialogue </summary>
    void EndDialogue()
    {
        Debug.Log("End dialogue index: " + currentDialogueIndex);
        AnimateDialogueBox(false);
        currentDialogueIndex++;

        if (currentDialogueIndex >= dialogues.Length)
            EndDialogueChain();
        else
            StartDialogue();
    }

    void EndDialogueChain()
    {
        Debug.Log("End dialogue chain");
        dialogueChainIsActive = false;

        if (TriggerDialogueChainOnlyOnce)
        {
            canTriggerDialogueChain = false;
            // later save as a flag in system
        }
    }

    /// <summary> Animate the dialogue box if it exists </summary>
    void AnimateDialogueBox(bool state)
    {
        // later set things as hidden, 
        // and make the dialogue container grow in size, 
        // set delay until it's done
        // do in reverse if false
        if (dialogueBox != null)
            dialogueBox.SetActive(state);
    }

    /// <summary> Animate the next button if it exists </summary>
    void AnimateNextButton(bool state)
    {
        if (nextButton != null)
            nextButton.SetActive(state);
    }

    /// <summary> Helper.. get the delay based on the lines scroll speed </summary>
    float GetScrollSpeedDelay(Dialogue.ScrollSpeed speed)
    {
        return speed switch
        {
            Dialogue.ScrollSpeed.Slow => 0.08f,
            Dialogue.ScrollSpeed.Medium => 0.05f,
            Dialogue.ScrollSpeed.Fast => 0.02f,
            Dialogue.ScrollSpeed.ExtraFast => 0.005f,
            _ => 0.05f,
        };
    }

    /// <summary> Pause other player inputs like movement </summary>
    void PauseGameWorld() 
    {
        // TODO
        // implement later... to make sure player and enemies are not moving around, etc while dialogue plays
        // will likely need to get all gameobjects with a tag like (player, npc, enemy)
        // assign a public method called PauseCharacter(bool state) and make sure they don't move
    }
}
