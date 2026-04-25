using UnityEngine;
using UnityEngine.UI;

public class GameSettingsUI : MonoBehaviour
{
    private GameObject settingsPanel;
    private Font pixelFont;

    void Start()
    {
        // Use Legacy font to match the "basic/retro" look in your screenshot
        pixelFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        CreateSettingsButton();
    }

    void CreateSettingsButton()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        // Gear/Settings Button in Corner
        GameObject btnObj = new GameObject("Btn_Settings_Open", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.sizeDelta = new Vector2(120, 50);
        rect.anchoredPosition = new Vector2(-20, -20);

        btnObj.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
        
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(btnObj.transform, false);
        Text t = textObj.GetComponent<Text>();
        t.text = "SETTINGS";
        t.font = pixelFont;
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        ((RectTransform)textObj.transform).sizeDelta = rect.sizeDelta;

        btnObj.GetComponent<Button>().onClick.AddListener(ShowSettings);
    }

    public void ShowSettings()
    {
        if (settingsPanel == null) CreateSettingsPopup();
        settingsPanel.SetActive(true);
    }

    void CreateSettingsPopup()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        
        // 1. Dark Background Overlay
        settingsPanel = new GameObject("SettingsOverlay", typeof(RectTransform), typeof(Image));
        settingsPanel.transform.SetParent(canvas.transform, false);
        RectTransform overlayRect = settingsPanel.GetComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;
        settingsPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0.85f);

        // 2. Central Panel
        GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(settingsPanel.transform, false);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(500, 400);
        panel.GetComponent<Image>().color = new Color(0.12f, 0.12f, 0.12f, 1f);

        // 3. Header Text
        CreateLabel(panel.transform, "SETTINGS", new Vector2(0, 140), 40, Color.white);

        // 4. Music Row
        CreateSettingRow(panel.transform, "MUSIC", new Vector2(0, 40), "BGMEnabled", (val) => {
            if (AudioManager.Instance != null) AudioManager.Instance.SetBGM(val);
        });

        // 5. Sound Row
        CreateSettingRow(panel.transform, "SOUND", new Vector2(0, -60), "SFXEnabled", (val) => {
            if (AudioManager.Instance != null) AudioManager.Instance.SetSFX(val);
        });

        // 6. Back Button
        GameObject backBtn = new GameObject("BackBtn", typeof(RectTransform), typeof(Image), typeof(Button));
        backBtn.transform.SetParent(panel.transform, false);
        RectTransform backRect = backBtn.GetComponent<RectTransform>();
        backRect.anchoredPosition = new Vector2(0, -140);
        backRect.sizeDelta = new Vector2(180, 60);
        backBtn.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1f);

        GameObject backTxt = new GameObject("Text", typeof(RectTransform), typeof(Text));
        backTxt.transform.SetParent(backBtn.transform, false);
        Text bt = backTxt.GetComponent<Text>();
        bt.text = "BACK";
        bt.font = pixelFont;
        bt.alignment = TextAnchor.MiddleCenter;
        bt.fontSize = 24;
        ((RectTransform)backTxt.transform).sizeDelta = backRect.sizeDelta;

        backBtn.GetComponent<Button>().onClick.AddListener(() => {
            settingsPanel.SetActive(false);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayPopupClose();
        });
    }

    void CreateSettingRow(Transform parent, string label, Vector2 pos, string prefsKey, UnityEngine.Events.UnityAction<bool> action)
    {
        GameObject row = new GameObject(label + "Row", typeof(RectTransform));
        row.transform.SetParent(parent, false);
        row.GetComponent<RectTransform>().anchoredPosition = pos;

        // Label
        GameObject lbl = new GameObject("Label", typeof(RectTransform), typeof(Text));
        lbl.transform.SetParent(row.transform, false);
        Text t = lbl.GetComponent<Text>();
        t.text = label;
        t.font = pixelFont;
        t.fontSize = 30;
        t.alignment = TextAnchor.MiddleLeft;
        lbl.GetComponent<RectTransform>().anchoredPosition = new Vector2(-80, 0);
        lbl.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);

        // Toggle (Standard Unity UI Toggle structure)
        GameObject toggleObj = new GameObject("Toggle", typeof(RectTransform), typeof(Toggle));
        toggleObj.transform.SetParent(row.transform, false);
        toggleObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, 0);
        
        Toggle toggle = toggleObj.GetComponent<Toggle>();
        toggle.isOn = PlayerPrefs.GetInt(prefsKey, 1) == 1;

        // Background of Toggle
        GameObject bg = new GameObject("Background", typeof(RectTransform), typeof(Image));
        bg.transform.SetParent(toggleObj.transform, false);
        bg.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);
        bg.GetComponent<Image>().color = Color.gray;

        // Checkmark
        GameObject check = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
        check.transform.SetParent(bg.transform, false);
        check.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        check.GetComponent<Image>().color = Color.green;
        
        toggle.targetGraphic = bg.GetComponent<Image>();
        toggle.graphic = check.GetComponent<Image>();

        toggle.onValueChanged.AddListener((val) => {
            PlayerPrefs.SetInt(prefsKey, val ? 1 : 0);
            PlayerPrefs.Save();
            action(val);
            if (AudioManager.Instance != null) AudioManager.Instance.PlayToggle();
        });
    }

    void CreateLabel(Transform parent, string text, Vector2 pos, int size, Color color)
    {
        GameObject obj = new GameObject("Label_" + text, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(parent, false);
        Text t = obj.GetComponent<Text>();
        t.text = text;
        t.font = pixelFont;
        t.fontSize = size;
        t.color = color;
        t.alignment = TextAnchor.MiddleCenter;
        obj.GetComponent<RectTransform>().anchoredPosition = pos;
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 100);
    }
}