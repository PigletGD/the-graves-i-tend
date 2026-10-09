using Ink.Parsed;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StoryEntry 1", menuName = "Scriptable Objects/StoryEntry"), System.Serializable]
public class StorySO : ScriptableObject
{
    [field: SerializeField] public TextAsset inkJsonFile { get; private set; }
    [field: SerializeField] public AudioClip[] voiceArray;
    
    private Dictionary<string, AudioClip> voiceOvers;

    public void FillDictionary()
    {
        voiceOvers = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in voiceArray)
        {
            voiceOvers.Add(clip.name, clip);
        }
    }

    public AudioClip GetAudioClipByName(string audioName)
    {
        AudioClip result;
        if(voiceOvers.TryGetValue(audioName, out result))
            return result;

        Debug.LogWarning("Audio File " + audioName + " was not found");
        return null;

    }
}
