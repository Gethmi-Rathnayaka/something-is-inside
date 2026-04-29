using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip achievementMusic;

    public AudioSource sfxSource;

    public AudioClip click;
    public AudioClip BadEnd;
    public AudioClip GoodEnd;
    public AudioClip NeutralEnd;
    public AudioClip Popup;
    public AudioClip unlocked;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Boot":
                PlayMusic(menuMusic);
                break;

            case "game":
                PlayMusic(gameMusic);
                break;

            case "save":
                PlayMusic(achievementMusic);
                break;
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayEndingSFX(EndingType endingType)
    {
        switch (endingType)
        {
            case EndingType.Good:
                PlaySFX(GoodEnd);
                break;
            case EndingType.Bad:
                PlaySFX(BadEnd);
                break;
            case EndingType.Neutral:
                PlaySFX(NeutralEnd);
                break;
        }
    }
}