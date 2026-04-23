using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPopup : MonoBehaviour
{
    private GameObject popup;
    private Text resultText;
    private Text durationText;
    
    public void Create(Canvas canvas)
    {
        popup = new GameObject("GameOverPopup", typeof(RectTransform), typeof(Image));
        popup.transform.SetParent(canvas.transform, false);
        
        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.sizeDelta = Vector2.zero;
        
        Image bgImage = popup.GetComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(popup.transform, false);
        
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(400, 300);
        panelRect.anchoredPosition = Vector2.zero;
        
        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = Color.white;
        
        GameObject resultObj = new GameObject("ResultText", typeof(RectTransform), typeof(Text));
        resultObj.transform.SetParent(panel.transform, false);
        
        RectTransform resultRect = resultObj.GetComponent<RectTransform>();
        resultRect.anchorMin = new Vector2(0.5f, 0.5f);
        resultRect.anchorMax = new Vector2(0.5f, 0.5f);
        resultRect.sizeDelta = new Vector2(300, 60);
        resultRect.anchoredPosition = new Vector2(0, 80);
        
        resultText = resultObj.GetComponent<Text>();
        resultText.fontSize = 32;
        resultText.alignment = TextAnchor.MiddleCenter;
        resultText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        resultText.color = Color.black;
        
        GameObject durationObj = new GameObject("DurationText", typeof(RectTransform), typeof(Text));
        durationObj.transform.SetParent(panel.transform, false);
        
        RectTransform durationRect = durationObj.GetComponent<RectTransform>();
        durationRect.anchorMin = new Vector2(0.5f, 0.5f);
        durationRect.anchorMax = new Vector2(0.5f, 0.5f);
        durationRect.sizeDelta = new Vector2(300, 40);
        durationRect.anchoredPosition = new Vector2(0, 30);
        
        durationText = durationObj.GetComponent<Text>();
        durationText.fontSize = 20;
        durationText.alignment = TextAnchor.MiddleCenter;
        durationText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        durationText.color = Color.black;
        
        CreateButton(panel.transform, "RetryBtn", new Vector2(-80, -50), new Color(0.2f, 0.6f, 0.2f), "RETRY", () => RestartGame());
        CreateButton(panel.transform, "ExitBtn", new Vector2(80, -50), new Color(0.8f, 0.2f, 0.2f), "EXIT", () => ExitToMenu());
        
        popup.SetActive(false);
    }
    
    void CreateButton(Transform parent, string name, Vector2 position, Color color, string text, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btn = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        btn.transform.SetParent(parent, false);
        
        RectTransform rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(120, 50);
        rect.anchoredPosition = position;
        
        Image btnImage = btn.GetComponent<Image>();
        btnImage.color = color;
        
        Button button = btn.GetComponent<Button>();
        button.onClick.AddListener(onClick);
        
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(btn.transform, false);
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        
        Text btnText = textObj.GetComponent<Text>();
        btnText.text = text;
        btnText.fontSize = 24;
        btnText.alignment = TextAnchor.MiddleCenter;
        btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        btnText.color = Color.white;
    }
    
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    void ExitToMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
    
    public void ShowGameOver(string winner, float duration)
    {
        if (winner == "Draw")
        {
            resultText.text = "DRAW!";
        }
        else
        {
            int playerNum = winner == "X" ? 1 : 2;
            resultText.text = $"PLAYER {playerNum} WINS!";
        }
        durationText.text = $"Time: {duration:F1}s";
        popup.SetActive(true);
    }
    
    public bool IsActive()
    {
        return popup != null && popup.activeSelf;
    }
}