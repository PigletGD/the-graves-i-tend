using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextAsset inkFile;     // should eventually become a parameter that is passed whenever the scene is loaded
    [SerializeField] private AudioClip storyAudio;

    private Story currentStory;
    private string currentDialogue;

    private bool isPaused;      // used for delays to time audio and text
    private bool isDialoguePlaying;     // controls whether panel is being displayed
    private int dialogueIndex;          // controls which part of the dialogue string it is currently at
    private int textIndex;      // used for how many fixed updates have passed
    private int textSpeed = 4;      // used to print a character after X amount of fixedupdate

    private void Start()
    {
        isDialoguePlaying = false;
        isPaused = false;

        if (inkFile != null)
        {

            StartStory(inkFile);
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
