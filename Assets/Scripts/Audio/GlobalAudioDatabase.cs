using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalAudioDatabase", menuName = "Audio/Global Database")]
public class GlobalAudioDatabase : ScriptableObject
{
    [Serializable]
    public class AudioDatabaseEntry
    {
        public string ID;
        public AudioClip Clip;
    }
    
    [SerializeField] private AudioDatabaseEntry[] BGMList;
    [SerializeField] private AudioDatabaseEntry[] SFXList;

    public Dictionary<string, AudioClip> BuildBGMDictionary()
    {
        if (BGMList == null || BGMList.Length == 0)
            return null;

        var dictionary = new Dictionary<string, AudioClip>();
        foreach (var bgm in BGMList)
            if (bgm.Clip != null)
                dictionary.TryAdd(bgm.ID, bgm.Clip);

        return dictionary;
    }
    
    public Dictionary<string, AudioClip> BuildSFXDictionary()
    {
        if (SFXList == null || SFXList.Length == 0)
            return null;

        var dictionary = new Dictionary<string, AudioClip>();
        foreach (var sfx in SFXList)
            if (sfx.Clip != null)
                dictionary.TryAdd(sfx.ID, sfx.Clip);

        return dictionary;
    }
}