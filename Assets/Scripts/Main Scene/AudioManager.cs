using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips")]
    public AudioClip music;
    public AudioClip buttonClick;
    public AudioClip newButton;
    public AudioClip popupClose;
    public AudioClip toggleSound;

    private AudioSource bgmSource;
    private AudioSource sfxSource;

    [HideInInspector] public bool bgmEnabled = true;
    [HideInInspector] public bool sfxEnabled = true;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CreateAudioSources();
    }

    void CreateAudioSources()
    {
        GameObject bgmObj = new GameObject("BGM_Source");
        bgmObj.transform.SetParent(transform);
        bgmSource = bgmObj.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;

        GameObject sfxObj = new GameObject("SFX_Source");
        sfxObj.transform.SetParent(transform);
        sfxSource = sfxObj.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
    }

    void Start()
    {
        // 1. Load saved preferences before doing anything else
        // (1 = enabled, 0 = disabled. Default to 1)
        bgmEnabled = PlayerPrefs.GetInt("BGM_On", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFX_On", 1) == 1;

        // 2. Now PlayMusic will respect the loaded bgmEnabled state
        PlayMusic();
    }

    // =========================
    // 🎵 BGM
    // =========================

    public void PlayMusic()
    {
        if (!bgmEnabled || music == null) return;

        bgmSource.clip = music;
        bgmSource.Play();
    }

    public void StopMusic()
    {
        bgmSource.Stop();
    }

    public void SetBGM(bool state)
    {
        bgmEnabled = state;
        
        // Save choice to disk
        PlayerPrefs.SetInt("BGM_On", state ? 1 : 0);
        PlayerPrefs.Save();

        if (state) 
        {
            if (!bgmSource.isPlaying) PlayMusic();
        }
        else 
        {
            StopMusic();
        }
    }

    // Alias for Toggle
    public void ToggleBGM(bool state) => SetBGM(state);

    // =========================
    // 🔊 SFX
    // =========================

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetSFX(bool state)
    {
        sfxEnabled = state;

        // Save choice to disk
        PlayerPrefs.SetInt("SFX_On", state ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Alias for Toggle
    public void ToggleSFX(bool state) => SetSFX(state);

    // =========================
    // 🔘 Helpers
    // =========================

    public void PlayButtonClick() => PlaySFX(buttonClick);
    public void PlayNewButton() => PlaySFX(newButton);
    public void PlayPopupClose() => PlaySFX(popupClose);
    public void PlayToggle() => PlaySFX(toggleSound);
}