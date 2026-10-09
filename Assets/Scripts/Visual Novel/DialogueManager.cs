using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerText;


    [SerializeField] private StorySO storySO;   // story to be played


    private Story currentStory;
    private string currentDialogue;

    private bool isPaused;      // used for delays to time audio and text
    private bool isDialoguePlaying;     // controls whether panel is being displayed
    private int dialogueIndex;          // controls which part of the dialogue string it is currently at
    private int textIndex;      // used for how many fixed updates have passed
    private int textSpeed = 4;      // used to print a character after X amount of fixedupdate

    #region Tags
    private const string SPEAKER_TAG = "speaker";
    private const string AUDIO_TAG = "audio";
    private const string DELAY_TAG = "delay";
    private const string DIALOGUE_TRANSITION_TAG = "dialogue_transition";
    #endregion

    private void Start()
    {
        isDialoguePlaying = false;
        isPaused = false;

        if (storySO != null)
        {

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
        textIndex = 0;
        dialogueIndex = 0;

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
        
        OverworldHandler.TargetContext = OverworldHandler.OverworldContext.Town;
        SceneManager.LoadScene("WorldInteractionScene");
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

                    break;
                case DELAY_TAG:
                    break;
                case DIALOGUE_TRANSITION_TAG:
                    break;
                default:
                    Debug.LogWarning("Tag is not a registered Tag key");
                    break;
            }
        }
    }


    private void UpdateDialogueText()
    {
        if (isDialoguePlaying && !isPaused)
        {
            textIndex++;

            // Controls the speed at which text is printed out
            if (textIndex >= textSpeed)
            {
                dialogueText.text += currentDialogue[dialogueIndex];
                dialogueIndex++;
                textIndex = 0;
            }

            if (dialogueText.text == currentDialogue)
            {
                isDialoguePlaying = false;
                StartCoroutine(DelayStoryContinue(2f));
            }
        }
    }

    private IEnumerator DelayStoryContinue(float delay)
    {
        yield return new WaitForSeconds(delay);
        ContinueStory();
    }

}
