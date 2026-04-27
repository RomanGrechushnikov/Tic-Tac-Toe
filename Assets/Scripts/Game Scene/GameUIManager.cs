using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Text timerText { get; private set; }
    public Text p1MovesText { get; private set; }
    public Text p2MovesText { get; private set; }
    private Font pixelFont;
    private GameSettingsUI settingsUI; // Direct reference

    public void CreateHUD(Canvas canvas)
    {
        pixelFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        // Initialize Settings Logic immediately
        settingsUI = gameObject.AddComponent<GameSettingsUI>();

        // Timer (VERY TOP MIDDLE)
        timerText = CreateHUDText(canvas, "Timer", new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, 0), "Time: 0.0");
        timerText.alignment = TextAnchor.UpperCenter;

        // P1/P2 Moves
        p1MovesText = CreateHUDText(canvas, "P1Moves", new Vector2(0, 1), new Vector2(0, 1), new Vector2(200, -20), "P1 (X): 0");
        p2MovesText = CreateHUDText(canvas, "P2Moves", new Vector2(1, 1), new Vector2(1, 1), new Vector2(-200, -20), "P2 (O): 0");

        CreateExitButton(canvas);
        CreateSettingsButton(canvas); // Pass canvas directly
    }

    private Text CreateHUDText(Canvas canvas, string name, Vector2 anchor, Vector2 pivot, Vector2 pos, string defaultText)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(canvas.transform, false);
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = pivot;
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(250, 50);

        Text t = obj.GetComponent<Text>();
        t.text = defaultText;
        t.font = pixelFont;
        t.fontSize = 22;
        t.color = Color.white;
        return t;
    }

    void CreateSettingsButton(Canvas canvas)
    {
        GameObject btnObj = new GameObject("Btn_Settings_Open", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);

        // Relative Scaling
        float btnWidth = Screen.width * 0.12f;
        float btnHeight = btnWidth * 0.4f;
        float margin = Screen.width * 0.02f;

        rect.sizeDelta = new Vector2(btnWidth, btnHeight);
        rect.anchoredPosition = new Vector2(-margin, -margin);

        btnObj.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 0.9f);
        
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(btnObj.transform, false);
        Text t = textObj.GetComponent<Text>();
        t.text = "SETTINGS";
        t.font = pixelFont;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = Mathf.RoundToInt(btnHeight * 0.4f);
        t.color = Color.white;
        ((RectTransform)textObj.transform).sizeDelta = rect.sizeDelta;

        // Use the direct reference instead of FindAnyObjectByType
        btnObj.GetComponent<Button>().onClick.AddListener(() => {
            if (settingsUI != null) settingsUI.ShowSettings();
        });
    }

    void CreateExitButton(Canvas canvas)
    {
        GameObject btnObj = new GameObject("ExitButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        
        float btnWidth = Screen.width * 0.12f;
        float btnHeight = btnWidth * 0.4f;
        float margin = Screen.width * 0.02f;

        rect.anchoredPosition = new Vector2(margin, -margin);
        rect.sizeDelta = new Vector2(btnWidth, btnHeight);

        btnObj.GetComponent<Image>().color = new Color(0.7f, 0.2f, 0.2f, 1f);
        btnObj.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(btnObj.transform, false);
        Text t = textObj.GetComponent<Text>();
        t.text = "EXIT";
        t.font = pixelFont;
        t.alignment = TextAnchor.MiddleCenter;
        t.fontSize = Mathf.RoundToInt(btnHeight * 0.5f);
        ((RectTransform)textObj.transform).sizeDelta = rect.sizeDelta;
    }
}