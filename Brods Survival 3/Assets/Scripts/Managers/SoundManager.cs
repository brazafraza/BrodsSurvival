using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class SoundInfo
{
    [Tooltip("Audio clip to play")]
    public AudioClip clip;
    [Tooltip("Mixer group to play audio on")]
    public AudioMixerGroup group;
    [Tooltip("Time to wait before playing sound after being played")]
    [Min(0.0f)]
    public float delay = 0.0f;

    [Tooltip("Volume to play the sound at (Percent)")]
    [Range(0.0f, 100.0f)]
    public float volumePercent = 100.0f;

    [Header("Pitch")]
    [Range(-3.0f, 3.0f)]
    public float min = 1;
    [Range(-3.0f, 3.0f)]
    public float max = 1;
}

public class MultiSoundInfo : SoundInfo
{
    public AudioClip clips;
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }


    public GameObject audioSourcePrefab;
    public int startingSoundObjects;

    // Combat
    [field: SerializeField] public SoundInfo Punch { get; private set; }
    [field: SerializeField] public SoundInfo GotDamaged { get; private set; }
    [field: SerializeField] public SoundInfo HeadGotDamaged { get; private set; }
    [field: SerializeField] public SoundInfo ArmBreak { get; private set; }
    [field: SerializeField] public SoundInfo HeadBreak { get; private set; }
    [field: SerializeField] public SoundInfo Headbutt { get; private set; }
    [field: SerializeField] public SoundInfo Parry { get; private set; }
    [field: SerializeField] public SoundInfo SuccessfulParry { get; private set; }
    [field: SerializeField] public SoundInfo StringSnap { get; private set; }
    [field: SerializeField] public SoundInfo FlailStart { get; private set; }


    //TODO: Movement sound, random from set of sounds
    [field: SerializeField] public SoundInfo Move { get; private set; }

    //UI
    [field: SerializeField] public SoundInfo Select { get; private set; }
    [field: SerializeField] public SoundInfo MoveSelection { get; private set; }
    [field: SerializeField] public SoundInfo CostumeChange { get; private set; }
    [field: SerializeField] public SoundInfo HeadChange { get; private set; }


    //Music / Ambient Tracks
    [field: SerializeField] public SoundInfo MainMenu { get; private set; }
    [field: SerializeField] public SoundInfo CharacterSelect { get; private set; }
    [field: SerializeField] public SoundInfo Gameplay { get; private set; }
    [field: SerializeField] public SoundInfo WinScreen { get; private set; }

    //Environment
    [field: SerializeField] public SoundInfo CurtainsOpen { get; private set; }
    [field: SerializeField] public SoundInfo CurtainsClose { get; private set; }
    [field: SerializeField] public SoundInfo PropSetup { get; private set; }
    [field: SerializeField] public SoundInfo PropCollision { get; private set; }
    [field: SerializeField] public SoundInfo Shush { get; private set; }
    [field: SerializeField] public SoundInfo Clapping { get; private set; }




    private List<AudioSource> audioSources = new();
    [SerializeField] private AudioSource musicTrack;
    private Dictionary<AudioSource, Transform> audioTransforms = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for (int i = 0; i < startingSoundObjects; i++)
        {
            NewAudioSource();
        }
        SwitchMusic(MainMenu);
    }
    public void Play(Vector3 position, SoundInfo soundInfo)
    {
        // Find Audio Source not playing any sound, create a new one if none avaliable
        AudioSource audioSource = null;
        foreach (AudioSource a in audioSources)
        {
            if (a.isPlaying) { continue; }
            audioSource = a;
            break;
        }
        if (audioSource == null)
        {
            audioSource = NewAudioSource();
        }

        audioSource.clip = soundInfo.clip;
        audioSource.outputAudioMixerGroup = soundInfo.group;
        audioTransforms[audioSource].position = position;
        audioSource.pitch = UnityEngine.Random.Range(soundInfo.min, soundInfo.max);
        audioSource.volume = soundInfo.volumePercent / 100.0f;
        audioSource.PlayDelayed(soundInfo.delay);
    }

    public void SwitchMusic(SoundInfo soundInfo)
    {
        //TODO: Check if this 'if' is actually needed, I do not know what happens if you reassign a clip
        if (soundInfo.clip != musicTrack.clip)
        {
            musicTrack.clip = soundInfo.clip;
            musicTrack.outputAudioMixerGroup = soundInfo.group;
            musicTrack.pitch = UnityEngine.Random.Range(soundInfo.min, soundInfo.max);
            musicTrack.volume = soundInfo.volumePercent / 100.0f;
            musicTrack.PlayDelayed(soundInfo.delay);
        }
    }

    private AudioSource NewAudioSource()
    {
        GameObject newAudioObject = Instantiate(audioSourcePrefab);
        AudioSource newAudioSource = newAudioObject.GetComponent<AudioSource>();
        audioSources.Add(newAudioSource);
        audioTransforms.Add(newAudioSource, newAudioObject.transform);
        newAudioObject.transform.parent = transform;
        return newAudioSource;
    }
}
