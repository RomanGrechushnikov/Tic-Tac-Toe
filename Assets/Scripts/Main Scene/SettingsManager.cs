using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPopup;
    public Toggle bgmToggle;
    public Toggle sfxToggle;

    void Start()
    {
        settingsPopup.SetActive(false);

        if (bgmToggle != null)
        {
            bgmToggle.isOn = PlayerPrefs.GetInt("BGMEnabled", 1) == 1;
            bgmToggle.onValueChanged.AddListener(OnBGMToggled);
        }

        if (sfxToggle != null)
        {
            sfxToggle.isOn = PlayerPrefs.GetInt("SFXEnabled", 1) == 1;
            sfxToggle.onValueChanged.AddListener(OnSFXToggled);
        }
    }

    public void ShowSettings()
    {
        settingsPopup.SetActive(true);
    }

    public void HideSettings()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPopupClose();

        settingsPopup.SetActive(false);
    }

    void OnBGMToggled(bool isOn)
    {
        PlayerPrefs.SetInt("BGMEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetBGM(isOn);
    }

    void OnSFXToggled(bool isOn)
    {
        PlayerPrefs.SetInt("SFXEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFX(isOn);
    }
}