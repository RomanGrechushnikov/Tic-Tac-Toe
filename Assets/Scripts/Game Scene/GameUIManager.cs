using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Text timerText { get; private set; }
    public Text p1MovesText { get; private set; }
    public Text p2MovesText { get; private set; }
    private Font pixelFont;
    private GameSettingsUI settingsUI;

    public void CreateHUD(Canvas canvas)
    {
        pixelFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Initialize Settings Logic
        settingsUI = gameObject.AddComponent<GameSettingsUI>();

        // 1. Define Relative Parameters based on Screen size
        float horizontalMargin = Screen.width * 0.04f;   // 4% margin
        float verticalMargin = Screen.height * 0.02f;   // 2% margin
        Vector2 textSize = new Vector2(Screen.width * 0.2f, Screen.height * 0.06f);
        int dynamicFontSize = Mathf.RoundToInt(Screen.height * 0.05f);

        // 2. Timer (Centered at the very top)
        timerText = CreateHUDText(canvas, "Timer", new Vector2(0.5f, 1), new Vector2(0.5f, 1), 
            new Vector2(0, -verticalMargin), "Time: 0.0", textSize, dynamicFontSize);
        timerText.alignment = TextAnchor.UpperCenter;

        // 3. P1 Moves (Top Left)
        p1MovesText = CreateHUDText(canvas, "P1Moves", new Vector2(0, 1), new Vector2(0, 1), 
            new Vector2(horizontalMargin, -verticalMargin), "Player 1: 0", textSize, dynamicFontSize);
        p1MovesText.alignment = TextAnchor.UpperLeft;

        // 4. P2 Moves (Top Right)
        p2MovesText = CreateHUDText(canvas, "P2Moves", new Vector2(1, 1), new Vector2(1, 1), 
            new Vector2(-horizontalMargin, -verticalMargin), "Player 2: 0", textSize, dynamicFontSize);
        p2MovesText.alignment = TextAnchor.UpperRight;

        // 5. Buttons
        CreateExitButton(canvas, horizontalMargin, verticalMargin);
        CreateSettingsButton(canvas, horizontalMargin, verticalMargin);
    }

    private Text CreateHUDText(Canvas canvas, string name, Vector2 anchor, Vector2 pivot, Vector2 pos, string defaultText, Vector2 size, int fontSize)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = pivot;
        rect.anchoredPosition = pos;
        rect.sizeDelta = size;

        Text t = obj.GetComponent<Text>();
        t.text = defaultText;
        t.font = pixelFont;
        t.fontSize = fontSize;
        t.color = Color.white;
        return t;
    }

    void CreateSettingsButton(Canvas canvas, float hMargin, float vMargin)
    {
        GameObject btnObj = new GameObject("Btn_Settings_Open", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0); // Bottom Right
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);

        float btnWidth = Screen.width * 0.12f;
        float btnHeight = btnWidth * 0.4f;

        rect.sizeDelta = new Vector2(btnWidth, btnHeight);
        rect.anchoredPosition = new Vector2(-hMargin, vMargin);

        btnObj.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
        
        CreateButtonText(btnObj.transform, "SETTINGS", btnHeight);

        btnObj.GetComponent<Button>().onClick.AddListener(() => {
            if (settingsUI != null) settingsUI.ShowSettings();
        });
    }

    void CreateExitButton(Canvas canvas, float hMargin, float vMargin)
    {
        GameObject btnObj = new GameObject("ExitButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0); // Bottom Left
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        
        float btnWidth = Screen.width * 0.12f;
        float btnHeight = btnWidth * 0.4f;

        rect.anchoredPosition = new Vector2(hMargin, vMargin);
        rect.sizeDelta = new Vector2(btnWidth, btnHeight);

        btnObj.GetComponent<Image>().color = new Color(0.7f, 0.2f, 0.2f, 1f);
        btnObj.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        CreateButtonText(btnObj.transform, "EXIT", btnHeight);
    }

    private void CreateButtonText(Transform parent, string label, float btnHeight)
    {
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(parent, false);
        RectTransform rect = textObj.GetComponent<RectTransform>();
        rect.sizeDelta = parent.GetComponent<RectTransform>().sizeDelta;

        Text t = textObj.GetComponent<Text>();
        t.text = label;
        t.font = pixelFont;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = Mathf.RoundToInt(btnHeight * 0.5f);
        t.color = Color.white;
    }
}