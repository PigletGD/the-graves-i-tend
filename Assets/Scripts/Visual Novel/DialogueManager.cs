using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;


    [SerializeField] private StorySO storySO;   // story to be played
    private AudioSource audioSource;

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


    private const float DEFAULT_TEXT_SPEED = 0.1f;
    private const float TEXT_SPEED_BIAS = 1.2f; // Just a base bias since ideally text should be a bit faster than voice

    #region Tags
    private const string SPEAKER_TAG = "speaker";
    private const string AUDIO_TAG = "audio";
    private const string DELAY_TAG = "delay";
    private const string DIALOGUE_TRANSITION_TAG = "dialogue_transition";
    private const string TEXT_SPEED_TAG = "text_speed";
    private const string TEXT_SPEED_MULTIPLIER = "text_speed_multiplier";       // needed for any excess voice length
    #endregion

    private void Start()
    {
        isDialoguePlaying = false;
        isPaused = false;
        isDelayed = false;

        audioSource = this.GetComponent<AudioSource>();

        if (storySO != null)
        {
            storySO.FillDictionary();
            StartStory(storySO.inkJsonFile);
        }
    }

    private void Update()
    {
        UpdateDialogueText();
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
        //delayIndex = 0;

        if (currentStory.canContinue)
        {
            currentDialogue = currentStory.Continue();
            dialogueText.text = "";
            dialoguePanel.SetActive(true);
            HandleTags(currentStory.currentTags);
            
            if(audioSource.clip != null)
                timePerCharacter = audioSource.clip.length / (currentDialogue.Length * TEXT_SPEED_BIAS * textSpeedMultiplier);    
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
                    audioSource.clip = storySO.GetAudioClipByName(tagValue);
                    audioSource.Play();
                    break;
                case DELAY_TAG:
                    break;
                case DIALOGUE_TRANSITION_TAG:
                    if (!float.TryParse(tagValue, out dialogueTransitionDelayValue))
                        dialogueTransitionDelayValue = 2f;
                    break;
                case TEXT_SPEED_TAG:
                    break;
                case TEXT_SPEED_MULTIPLIER:
                    if (!float.TryParse(tagValue, out textSpeedMultiplier))
                        textSpeedMultiplier = 1f;
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
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }
}
