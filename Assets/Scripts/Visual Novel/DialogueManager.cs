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
    private int delayIndex;
    private int textIndex;      // used for how many fixed updates have passed, the lower the value
    private int textSpeed = 12;      // text index increments
    private float dialogueTransitionDelayValue = 2f;

    private const int TEXT_PRINT_VALUE = 60; // the value that needs to be achieved for text index to print the next character;

    #region Tags
    private const string SPEAKER_TAG = "speaker";
    private const string AUDIO_TAG = "audio";
    private const string DELAY_TAG = "delay";
    private const string DIALOGUE_TRANSITION_TAG = "dialogue_transition";
    private const string TEXT_SPEED_TAG = "text_speed";
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

    private void FixedUpdate()
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
        textIndex = 0;
        dialogueIndex = 0;
        delayIndex = 0;

        if (currentStory.canContinue)
        {
            currentDialogue = currentStory.Continue();
            dialogueText.text = "";
            dialoguePanel.SetActive(true);
            HandleTags(currentStory.currentTags);
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
                    break;
                case TEXT_SPEED_TAG:
                    int.TryParse(tagValue, out textSpeed);
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
            textIndex+= textSpeed;

            // Controls the speed at which text is printed out
            if (textIndex >= TEXT_PRINT_VALUE)
            {
                dialogueText.text += currentDialogue[dialogueIndex];
                dialogueIndex++;
                textIndex = 0;
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
