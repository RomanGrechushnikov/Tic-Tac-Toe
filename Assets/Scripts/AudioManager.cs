using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    
    [Header("Audio Clips")]
    public AudioClip newButton;
    public AudioClip buttonClick;
    public AudioClip placement;
    public AudioClip strike;
    public AudioClip popupOpen;
    public AudioClip popupClose;
    
    private bool bgmEnabled = true;
    private bool sfxEnabled = true;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        LoadSettings();
        ApplySettings();
    }
    
    void LoadSettings()
    {
        bgmEnabled = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
        sfxEnabled = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
    }
    
    void ApplySettings()
    {
        if (bgmSource != null)
            bgmSource.mute = !bgmEnabled;
        if (sfxSource != null)
            sfxSource.mute = !sfxEnabled;
    }
    
    public void SetBGM(bool enabled)
    {
        bgmEnabled = enabled;
        PlayerPrefs.SetInt("BGMEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
        if (bgmSource != null)
            bgmSource.mute = !enabled;
    }
    
    public void SetSFX(bool enabled)
    {
        sfxEnabled = enabled;
        PlayerPrefs.SetInt("SFXEnabled", enabled ? 1 : 0);
        PlayerPrefs.Save();
        if (sfxSource != null)
            sfxSource.mute = !enabled;
    }
    
    public bool IsBGMEnabled() => bgmEnabled;
    public bool IsSFXEnabled() => sfxEnabled;
    
    public void PlayNewButton()
    {
        if (sfxEnabled && newButton != null && sfxSource != null)
            sfxSource.PlayOneShot(newButton);
    }

    public void PlayButtonClick()
    {
        if (sfxEnabled && buttonClick != null)
            sfxSource.PlayOneShot(buttonClick);
    }
    
    public void PlayPlacement()
    {
        if (sfxEnabled && placement != null)
            sfxSource.PlayOneShot(placement);
    }
    
    public void PlayStrike()
    {
        if (sfxEnabled && strike != null)
            sfxSource.PlayOneShot(strike);
    }
    
    public void PlayPopupOpen()
    {
        if (sfxEnabled && popupOpen != null)
            sfxSource.PlayOneShot(popupOpen);
    }
    
    public void PlayPopupClose()
    {
        if (sfxEnabled && popupClose != null)
            sfxSource.PlayOneShot(popupClose);
    }
}
