using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundClip
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Volume")]
    [Range(0, 10)] public float masterVolume = 10f;//전체
    [Range(0, 10)] public float bgmVolume = 10f;//배경
    [Range(0, 10)] public float seVolume = 10f;//효과

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource seSource;

    [Header("Clip")]
    public List<SoundClip> bgmClips;
    public List<SoundClip> seClips;

    private Dictionary<string, AudioClip> bgmDict;
    private Dictionary<string, AudioClip> seDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeDictionaries()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        foreach (var soundClip in bgmClips)
        {
            if (!bgmDict.ContainsKey(soundClip.name))
                bgmDict.Add(soundClip.name, soundClip.clip);
        }

        seDict = new Dictionary<string, AudioClip>();
        foreach (var soundClip in seClips)
        {
            if (!seDict.ContainsKey(soundClip.name))
                seDict.Add(soundClip.name, soundClip.clip);
        }
    }

    void Update()
    {
        bgmSource.volume = masterVolume * bgmVolume;
        seSource.volume = masterVolume * seVolume;
    }

    public void PlayBGM(string name)
    {
        if (bgmDict.TryGetValue(name, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void PlaySE(string name)
    {
        if (seDict.TryGetValue(name, out AudioClip clip))
        {
            seSource.PlayOneShot(clip);
        }
    }

    public void SetMasterVolume(float value) => masterVolume = value;
    public void SetBGMVolume(float value) => bgmVolume = value;
    public void SetSEVolume(float value) => seVolume = value;
}