using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Goes through all available dialogue in the dialogue manager
/// 
/// Should be able to do the following:
/// - Read through list of dialogue
/// - Can be triggered by interacting within the trigger area (E)
/// - Automatically triggered by entering an area
/// - Can be made to trigger only once
/// </summary>
public class DialogueManager : MonoBehaviour
{
    /// <summary> Represents a unit of dialogue by a single character </summary>
    [System.Serializable]
    public class Dialogue
    {
        /// <summary> Represents an single line of dialogue </summary>
        [System.Serializable]
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
    public bool triggerDialogueByEnteringArea = false;
    public bool triggerDialogueByInteraction = false;
    public bool triggerDialogueOnlyOnce = false;

    [Header("Dialogue Display Settings")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI characterNameText;
    public GameObject nextButton;

    [Header("All Dialogue")]
    public Dialogue[] dialogues;
    
    // private variables
    private PlayerInput playerInput;    // to see if player has clicked an interact/next button
    private bool isInteractPressed = false; // if the interact button has been pressed
    private bool hasTriggered = false;  // register if dialogue has been triggered
    private bool canSeeDialogue = true;    // if player can see dialogue 
    private Coroutine dialogueCoroutine;
    private int currentLineIndex = 0;
    private int currentDialogueIndex = 0;
    private bool isTyping = false;

    void Awake()
    {
        if (!dialogueBox || dialogueText || characterNameText || nextButton)
        {
            Debug.LogError("Dialogue box components are missing. Please add via the inspector");
            return;
        }

        SetUpPlayerInput();

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
        isInteractPressed = context.ReadValueAsButton();
    }

    void Update()
    {
        // for now, we will see if the dialogue has been marked as "hasTriggered"
        // manually mark this in the inspector for testing
        // later implement trigger based on collision, etc for triggers
        if (hasTriggered && canSeeDialogue)
            StartDialogue(dialogues[currentDialogueIndex]);
        
        if (isInteractPressed)
            OnNextPressed();
    }

    // void StartDialogueChain()
    // {
    //     for (dialogues[currentDialogueIndex])
    // }

    void StartDialogue(Dialogue dialogue)
    {
        AnimateDialogueBox(true);   // open up dialogue box
        characterNameText.text = dialogue.characterName;    // set character name
        currentLineIndex = 0;   // start the line index in the dialogue
        if (dialogueCoroutine != null)
            StopCoroutine(dialogueCoroutine);

        dialogueCoroutine = StartCoroutine(TypeLine(dialogue.lines[currentLineIndex]));
    }

    /// <summary> Type the line of dialogue </summary>
    IEnumerator TypeLine(Dialogue.Line line)
    {   
        // start typing line
        isTyping = true;
        dialogueText.text = ""; // set dialogue text to empty first
        float delay = GetScrollSpeedDelay(line.scrollSpeed);    // get the scroll speed

        // add a letter to the dialogue text based on scroll speed
        foreach (char letter in line.text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(delay);
        }

        // no longer typing
        isTyping = false;
        AnimateNextButton(true); // show "Next" button once line is done
    }

    /// <summary> Player triggers next set of dialogue </summary>
    public void OnNextPressed()
    {
        // later add a skip function
        // for now don't do anything
        if (isTyping) return;

        // start next line
        currentLineIndex++;
        if (currentLineIndex < dialogues[currentDialogueIndex].lines.Length)
        {
            AnimateNextButton(false);
            if (dialogueCoroutine != null)
                StopCoroutine(dialogueCoroutine);

            dialogueCoroutine = StartCoroutine(TypeLine(dialogues[currentDialogueIndex].lines[currentLineIndex]));
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary> End the dialogue </summary>
    void EndDialogue()
    {
        AnimateDialogueBox(false);
        currentDialogueIndex++;

        if (triggerDialogueOnlyOnce)
        {
            hasTriggered = true;
            // later save as a flag in system
        }
    }

    /// <summary> Animate the dialogue box if it exists </summary>
    void AnimateDialogueBox(bool state)
    {
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
