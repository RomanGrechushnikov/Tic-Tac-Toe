using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPopupGenerator : MonoBehaviour
{
    private GameObject popup;

    public void CreateSettingsPopup()
    {
        // 1. Find Canvas
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        // 2. Main Root
        popup = new GameObject("PopupSettings");
        popup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = popup.AddComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.sizeDelta = Vector2.zero;

        // 3. Dark Overlay
        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(popup.transform, false);
        overlay.AddComponent<RectTransform>().anchorMin = Vector2.zero;
        overlay.GetComponent<RectTransform>().anchorMax = Vector2.one;
        overlay.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        overlay.AddComponent<Image>().color = new Color(0, 0, 0, 0.6f);

        // 4. Panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(popup.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(400, 300);
        panel.AddComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f);

        // 5. Title
        CreateText(panel.transform, "SETTINGS", new Vector2(0, 110), 32);

        // 6. Toggles (BGM & SFX)
        CreateToggle(panel.transform, "Music (BGM)", new Vector2(0, 30), "BGMEnabled", (isOn) => {
            if (AudioManager.Instance != null) AudioManager.Instance.SetBGM(isOn);
        });

        CreateToggle(panel.transform, "Sound (SFX)", new Vector2(0, -40), "SFXEnabled", (isOn) => {
            if (AudioManager.Instance != null) AudioManager.Instance.SetSFX(isOn);
        });

        // 7. Close Button
        GameObject closeBtnObj = new GameObject("Btn_Close", typeof(RectTransform), typeof(Image), typeof(Button));
        closeBtnObj.transform.SetParent(panel.transform, false);
        RectTransform btnRect = closeBtnObj.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(160, 50);
        btnRect.anchoredPosition = new Vector2(0, -110);
        closeBtnObj.GetComponent<Image>().color = Color.gray;
        
        CreateText(closeBtnObj.transform, "BACK", Vector2.zero, 24);
        
        closeBtnObj.GetComponent<Button>().onClick.AddListener(() => {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayPopupClose();
            Destroy(popup);
        });
    }

    void CreateToggle(Transform parent, string label, Vector2 pos, string prefsKey, UnityEngine.Events.UnityAction<bool> onToggle)
    {
        GameObject toggleObj = new GameObject(label);
        toggleObj.transform.SetParent(parent, false);
        toggleObj.AddComponent<RectTransform>().anchoredPosition = pos;

        // Label
        CreateText(toggleObj.transform, label, new Vector2(-60, 0), 20);

        // Background
        GameObject bg = new GameObject("BG", typeof(Image), typeof(Toggle));
        bg.transform.SetParent(toggleObj.transform, false);
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        bg.GetComponent<RectTransform>().anchoredPosition = new Vector2(80, 0);
        
        // Checkmark
        GameObject check = new GameObject("Checkmark", typeof(Image));
        check.transform.SetParent(bg.transform, false);
        check.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        check.GetComponent<Image>().color = Color.green;

        Toggle t = bg.GetComponent<Toggle>();
        t.graphic = check.GetComponent<Image>();
        t.isOn = PlayerPrefs.GetInt(prefsKey, 1) == 1;

        t.onValueChanged.AddListener((isOn) => {
            PlayerPrefs.SetInt(prefsKey, isOn ? 1 : 0);
            PlayerPrefs.Save();
            onToggle(isOn);
        });
    }

    void CreateText(Transform parent, string content, Vector2 pos, int size)
    {
        GameObject txtObj = new GameObject("Text", typeof(TextMeshProUGUI));
        txtObj.transform.SetParent(parent, false);
        var tmp = txtObj.GetComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = size;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.GetComponent<RectTransform>().anchoredPosition = pos;
        tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 50);
    }
}