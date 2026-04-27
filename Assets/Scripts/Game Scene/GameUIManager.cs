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
        settingsUI = gameObject.AddComponent<GameSettingsUI>();

        float horizontalMargin = Screen.width * 0.04f; 
        // Reduced vertical margin to move HUD higher
        float verticalMargin = Screen.height * 0.015f; 

        float sideTextWidth = Screen.width * 0.25f;
        float centerTextWidth = Screen.width * 0.3f;
        float hudHeight = Screen.height * 0.06f;

        int dynamicFontSize = Mathf.RoundToInt(Mathf.Min(Screen.width, Screen.height) * 0.05f);

        // 2. Timer - Adjusted position to be closer to the top edge
        timerText = CreateHUDText(canvas, "Timer", new Vector2(0.5f, 1), new Vector2(0.5f, 1), 
            new Vector2(0, -verticalMargin), "Time: 0.0", 
            new Vector2(centerTextWidth, hudHeight), dynamicFontSize);
        timerText.alignment = TextAnchor.UpperCenter;

        // 3. P1 Moves
        p1MovesText = CreateHUDText(canvas, "P1Moves", new Vector2(0, 1), new Vector2(0, 1), 
            new Vector2(horizontalMargin, -verticalMargin), "Player 1: 0", 
            new Vector2(sideTextWidth, hudHeight), dynamicFontSize);
        p1MovesText.alignment = TextAnchor.UpperLeft;

        // 4. P2 Moves
        p2MovesText = CreateHUDText(canvas, "P2Moves", new Vector2(1, 1), new Vector2(1, 1), 
            new Vector2(-horizontalMargin, -verticalMargin), "Player 2: 0", 
            new Vector2(sideTextWidth, hudHeight), dynamicFontSize);
        p2MovesText.alignment = TextAnchor.UpperRight;

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
        // Ensure text doesn't wrap if it exceeds the width
        t.horizontalOverflow = HorizontalWrapMode.Overflow; 
        return t;
    }

    void CreateSettingsButton(Canvas canvas, float hMargin, float vMargin)
    {
        GameObject btnObj = new GameObject("Btn_Settings_Open", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 0); 
        rect.anchorMax = new Vector2(1, 0);
        rect.pivot = new Vector2(1, 0);

        // Button width based on display width
        float btnWidth = Mathf.Min(Screen.width, Screen.height) * 0.30f;
        float btnHeight = Screen.height * 0.11f;

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
        rect.anchorMin = new Vector2(0, 0); 
        rect.anchorMax = new Vector2(0, 0);
        rect.pivot = new Vector2(0, 0);
        
        float btnWidth = Mathf.Min(Screen.width, Screen.height) * 0.30f;
        float btnHeight = Screen.height * 0.11f;

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
        t.fontSize = Mathf.RoundToInt(btnHeight * 0.4f);
        t.color = Color.white;
    }
}