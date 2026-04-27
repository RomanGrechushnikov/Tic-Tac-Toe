using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Text timerText { get; private set; }
    public Text p1MovesText { get; private set; }
    public Text p2MovesText { get; private set; }
    
    public void CreateHUD(Canvas canvas)
    {
        // P1 Moves (Top-Left)
        p1MovesText = CreateHUDText(canvas, "P1Moves", new Vector2(0, 1), new Vector2(0, 1), new Vector2(200, -20), "P1 (X): 0");
        p1MovesText.alignment = TextAnchor.MiddleLeft;

        // Timer (Top-Center)
        timerText = CreateHUDText(canvas, "Timer", new Vector2(0.5f, 1), new Vector2(0.5f, 1), new Vector2(0, -20), "Time: 0.0");
        timerText.alignment = TextAnchor.MiddleCenter;

        // P2 Moves (Top-Right)
        p2MovesText = CreateHUDText(canvas, "P2Moves", new Vector2(1, 1), new Vector2(1, 1), new Vector2(-200, -20), "P2 (O): 0");
        p2MovesText.alignment = TextAnchor.MiddleRight;

        CreateExitButton(canvas);
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
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 22;
        t.color = Color.white;
        
        return t;
    }

    void CreateExitButton(Canvas canvas)
    {
        GameObject btnObj = new GameObject("ExitButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        
        // Scale relative to WIDTH for horizontal consistency
        float btnWidth = Screen.width * 0.12f;  // 12% of screen width
        float btnHeight = btnWidth * 0.4f;      // Maintain a 2.5:1 aspect ratio
        float margin = Screen.width * 0.02f;    // 2% margin from the left edge
        
        rect.anchoredPosition = new Vector2(margin, -margin);
        rect.sizeDelta = new Vector2(btnWidth, btnHeight);

        btnObj.GetComponent<Image>().color = new Color(0.7f, 0.2f, 0.2f, 1f);
        btnObj.GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));

        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(btnObj.transform, false);
        Text t = textObj.GetComponent<Text>();
        t.text = "EXIT";
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.alignment = TextAnchor.MiddleCenter;
        
        // Font size relative to the new button height
        t.fontSize = Mathf.RoundToInt(btnHeight * 0.5f); 
        ((RectTransform)textObj.transform).sizeDelta = rect.sizeDelta;
    }
}