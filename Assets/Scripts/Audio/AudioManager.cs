using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                var go = new GameObject("AudioManager");
                var newInstance = go.AddComponent<AudioManager>();
                newInstance.InitializeAsInstance();
            }

            return instance;
        }
    }
    
    [SerializeField] private GlobalAudioDatabase globalAudioDatabase;
    
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    private Dictionary<string, AudioClip> bgmDicitionary = new();
    private Dictionary<string, AudioClip> sfxDicitionary = new();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        InitializeAsInstance();
        
        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
        
        if (globalAudioDatabase == null)
            globalAudioDatabase = Resources.Load<GlobalAudioDatabase>("GlobalAudioDatabase");
        
        BuildAudioDictionaries();
    }

    private void InitializeAsInstance()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void BuildAudioDictionaries()
    {
        if (globalAudioDatabase == null)
        {
            Debug.LogError("Could not find Global Audio Database. Will not be able to build audio dictionaries");
            return;
        }

        bgmDicitionary = globalAudioDatabase.BuildBGMDictionary();
        sfxDicitionary = globalAudioDatabase.BuildSFXDictionary();
    }
    
    public static void PlayBGMLoop(string id, float volume = 1.0f)
    {
        if (Instance.bgmDicitionary?.TryGetValue(id, out var clip) ?? false)
            PlayBGMLoop(clip, volume);
    }

    public static void PlayBGMLoop(AudioClip clip, float volume = 1.0f)
    {
        var source = Instance.bgmSource;
        
        source.Stop();
        
        source.clip = clip;
        source.loop = true;
        source.volume = volume;
        source.Play();
    }
    
    public static void PlaySFXOneShot(string id, float volume = 1.0f)
    {
        if (Instance.sfxDicitionary?.TryGetValue(id, out var clip) ?? false)
            PlaySFXOneShot(clip);
    }
    
    public static void PlaySFXOneShot(AudioClip clip, float volume = 1.0f)
    {
        var source = Instance.sfxSource;
        source.PlayOneShot(clip);
    }
}
