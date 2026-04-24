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

    public bool bgmEnabled = true;
    public bool sfxEnabled = true;

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

    public void ToggleBGM(bool state)
    {
        bgmEnabled = state;

        if (state) PlayMusic();
        else StopMusic();
    }

    // alias for UI compatibility
    public void SetBGM(bool state)
    {
        ToggleBGM(state);
    }

    // =========================
    // 🔊 SFX
    // =========================

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled || clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void ToggleSFX(bool state)
    {
        sfxEnabled = state;
    }

    // alias for UI compatibility
    public void SetSFX(bool state)
    {
        ToggleSFX(state);
    }

    // =========================
    // 🔘 Helpers
    // =========================

    public void PlayButtonClick() => PlaySFX(buttonClick);
    public void PlayNewButton() => PlaySFX(newButton);
    public void PlayPopupClose() => PlaySFX(popupClose);
    public void PlayToggle() => PlaySFX(toggleSound);
}