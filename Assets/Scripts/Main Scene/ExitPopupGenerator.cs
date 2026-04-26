using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExitPopupGenerator : MonoBehaviour
{
    private GameObject popup;

    public void CreateExitPopup()
    {
        // 1. Find Canvas dynamically (Matches ThemePopupCreator logic)
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        // 2. Main Root Object (PopupExit)
        popup = new GameObject("PopupExit");
        popup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = popup.AddComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.sizeDelta = Vector2.zero;

        // 3. Dark Overlay
        GameObject overlay = new GameObject("OverlayExit");
        overlay.transform.SetParent(popup.transform, false);
        RectTransform overlayRect = overlay.AddComponent<RectTransform>();
        overlayRect.anchorMin = Vector2.zero;
        overlayRect.anchorMax = Vector2.one;
        overlayRect.sizeDelta = Vector2.zero;
        Image overlayImg = overlay.AddComponent<Image>();
        overlayImg.color = new Color(0, 0, 0, 0.6f);

        // 4. Panel (PanelExit)
        GameObject panel = new GameObject("PanelExit");
        panel.transform.SetParent(popup.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(450, 250);
        panelRect.anchoredPosition = Vector2.zero;
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.15f, 0.15f, 0.15f, 1f);

        // 5. Message Text
        GameObject messageObj = new GameObject("Message", typeof(RectTransform), typeof(TextMeshProUGUI));
        messageObj.transform.SetParent(panel.transform, false);
        RectTransform msgRect = messageObj.GetComponent<RectTransform>();
        msgRect.anchoredPosition = new Vector2(0, 40);
        msgRect.sizeDelta = new Vector2(400, 100);
        TextMeshProUGUI msgText = messageObj.GetComponent<TextMeshProUGUI>();
        msgText.text = "EXIT GAME?";
        msgText.fontSize = 32;
        msgText.alignment = TextAlignmentOptions.Center;
        msgText.color = Color.white;

        // 6. Buttons
        CreateExitButton(panel.transform, "Btn_Yes", new Vector2(-100, -60), "YES", () => {
            Application.Quit();
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        });

        CreateExitButton(panel.transform, "Btn_No", new Vector2(100, -60), "NO", () => {
            Destroy(popup); // Completely removes the hierarchy
        });
    }

    void CreateExitButton(Transform parent, string name, Vector2 position, string label, UnityEngine.Events.UnityAction action)
    {
        GameObject btnObj = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(parent, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(140, 50);
        rect.anchoredPosition = position;
        
        btnObj.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
        btnObj.GetComponent<Button>().onClick.AddListener(action);

        // Button Text
        GameObject textObj = new GameObject("Text (TMP)", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(btnObj.transform, false);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = rect.sizeDelta;
        
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.black;
    }
}