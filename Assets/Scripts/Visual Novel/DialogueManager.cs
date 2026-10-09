using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private GameObject pauseText;

    [SerializeField] private StorySO storySO;   // story to be played
    private AudioSource voiceOverSource;

    private Story currentStory;
    private string currentDialogue;

    private bool isPaused;      // used for delays to time audio and text
    private bool isDialoguePlaying;     // controls whether panel is being displayed
    private bool isDelayed;     // bool for pausing the text printing

    private int dialogueIndex;          // controls which part of the dialogue string it is currently at
    private float timeElapsed;      
    private float timePerCharacter;
    //private int delayIndex;
    private float dialogueTransitionDelayValue = 2f;
    private float textSpeedMultiplier = 1f;

    private InputAction pauseAction;


    private const float DEFAULT_TEXT_SPEED = 0.1f;
    private const float TEXT_SPEED_BIAS = 1.2f; // Just a base bias since ideally text should be a bit faster than voice

    #region Tags
    private const string SPEAKER_TAG = "speaker";
    private const string AUDIO_TAG = "audio";
    private const string DELAY_TAG = "delay";
    private const string DIALOGUE_TRANSITION_TAG = "dialogue_transition";
    private const string FONT_STYLE_TAG = "font_style";     // bold, normal, italicized
    private const string TEXT_SPEED_TAG = "text_speed";
    private const string TEXT_SPEED_MULTIPLIER_TAG = "text_speed_multiplier";       // needed for any excess voice length
    #endregion



    private void Awake()
    {
        pauseAction = InputSystem.actions.FindAction("VN Pausing");
    }

    private void Start()
    {
        isDialoguePlaying = false;
        isPaused = false;
        isDelayed = false;

        voiceOverSource = this.GetComponent<AudioSource>();

        if (storySO != null)
        {
            storySO.FillDictionary();
            StartStory(storySO.inkJsonFile);
        }
    }

    private void Update()
    {
        HandleInput();
        UpdateDialogueText();
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("UI").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("UI").Disable();
    }

    public void StartStory(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        ContinueStory();
    }

    private void ContinueStory()
    {
        isDialoguePlaying = true;
        isPaused = false;
        isDelayed = false;
        timeElapsed = 0;
        dialogueIndex = 0;
        textSpeedMultiplier = 1f;
        dialogueText.fontStyle = FontStyles.Normal;
        //delayIndex = 0;

        if (currentStory.canContinue)
        {
            currentDialogue = currentStory.Continue();
            dialogueText.text = "";
            dialoguePanel.SetActive(true);
            HandleTags(currentStory.currentTags);
            
            if(voiceOverSource.clip != null)
                timePerCharacter = voiceOverSource.clip.length / (currentDialogue.Length * TEXT_SPEED_BIAS * textSpeedMultiplier);    
            else
                timePerCharacter = DEFAULT_TEXT_SPEED;
        }
            
        else
            EndStory();
            
    }

    private void EndStory()
    {
        isDialoguePlaying = false;
        currentDialogue = "";
        dialoguePanel.gameObject.SetActive(false);
    }

    private void PauseStory()
    {
        pauseText.gameObject.SetActive(true);
        isPaused = true;
        if (voiceOverSource.clip != null)
            voiceOverSource.Pause();
    }
    private void UnpauseStory()
    {
        pauseText.gameObject.SetActive(false);
        isPaused = false;
        if (voiceOverSource.clip != null)
            voiceOverSource.UnPause();
    }

    private void HandleInput()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            if (isPaused)
                UnpauseStory();
            else
                PauseStory();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag Could not be approriately parsed:" + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speakerText.text = tagValue;
                    break;
                case AUDIO_TAG:
                    voiceOverSource.clip = storySO.GetAudioClipByName(tagValue);
                    voiceOverSource.Play();
                    break;
                case DELAY_TAG:
                    break;
                case DIALOGUE_TRANSITION_TAG:
                    if (!float.TryParse(tagValue, out dialogueTransitionDelayValue))
                        dialogueTransitionDelayValue = 2f;
                    break;
                case TEXT_SPEED_TAG:
                    break;
                case TEXT_SPEED_MULTIPLIER_TAG:
                    if (!float.TryParse(tagValue, out textSpeedMultiplier))
                        textSpeedMultiplier = 1f;
                    break;
                case FONT_STYLE_TAG:
                    switch (tagValue)
                    {
                        case "bold":
                            dialogueText.fontStyle = FontStyles.Bold;
                            break;
                        case "normal":
                            dialogueText.fontStyle = FontStyles.Normal;
                            break;
                        case "italic":
                            dialogueText.fontStyle = FontStyles.Italic;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Debug.LogWarning("Tag is not a registered Tag key");
                    break;
            }
        }
    }


    private void UpdateDialogueText()
    {
        if (isDialoguePlaying && !isPaused && !isDelayed)
        {
            timeElapsed += Time.deltaTime;

            // Controls the speed at which text is printed out
            if (timeElapsed >= timePerCharacter)
            {
                dialogueText.text += currentDialogue[dialogueIndex];
                dialogueIndex++;
                timeElapsed -= timePerCharacter;
            }

            if (dialogueText.text == currentDialogue)
            {
                isDialoguePlaying = false;
                StartCoroutine(DelayStoryContinue(dialogueTransitionDelayValue));
            }
        }
    }

    private IEnumerator DelayStoryContinue(float delay)
    {
        yield return new WaitForSeconds(delay);
        ContinueStory();
    }

    private IEnumerator DelayDialogue(float delay)
    { 
        yield return new WaitForSeconds(delay);
        isDelayed = false;
    }

    private IEnumerator DelayAudio(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (voiceOverSource.clip != null)
        {
            voiceOverSource.Play();
        }
    }
}
