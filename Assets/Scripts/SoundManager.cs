using UnityEngine;

public class SoundManager : MonoBehaviour
{
 
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get { return instance; }
        private set { }
    }

    private SoundManager() { }

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip buttonClickSound;
    public AudioClip radioButtonSound;
    public AudioClip flipCardSound;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float backgroundVolume = 0.2f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private AudioSource backgroundSource;
    private AudioSource sfxSource;

    void Awake()
    {   
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupAudioSources()
    {
        backgroundSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Background audio settings
        backgroundSource.clip = backgroundMusic;
        backgroundSource.loop = true;
        backgroundSource.volume = backgroundVolume;
        backgroundSource.playOnAwake = false;

        // SFX audio settings
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume;

        backgroundSource.Play();
    }

    public void PlayButtonClick()
    {
        sfxSource.PlayOneShot(buttonClickSound, sfxVolume);
    }

    public void PlayRadioButtonClick()
    {
        sfxSource.PlayOneShot(radioButtonSound, sfxVolume);
    }

    public void PlayFLipCardSound() 
    {
        sfxSource.PlayOneShot(flipCardSound, sfxVolume);
    }
  
}
