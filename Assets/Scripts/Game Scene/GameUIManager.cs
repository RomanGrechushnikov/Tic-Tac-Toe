using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public Text timerText { get; private set; }
    public Text p1MovesText { get; private set; }
    public Text p2MovesText { get; private set; }
    
    public void CreateHUD(Canvas canvas)
    {
        // Timer
        GameObject timerObj = new GameObject("TimerText", typeof(RectTransform), typeof(Text));
        timerObj.transform.SetParent(canvas.transform, false);
        RectTransform timerRect = timerObj.GetComponent<RectTransform>();
        timerRect.anchorMin = new Vector2(1, 1);
        timerRect.anchorMax = new Vector2(1, 1);
        timerRect.anchoredPosition = new Vector2(-100, -50);
        timerRect.sizeDelta = new Vector2(200, 50);
        timerText = timerObj.GetComponent<Text>();
        timerText.text = "Time: 0.0";
        timerText.fontSize = 24;
        timerText.alignment = TextAnchor.MiddleRight;
        timerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // Player 1 moves
        GameObject p1Obj = new GameObject("P1MovesText", typeof(RectTransform), typeof(Text));
        p1Obj.transform.SetParent(canvas.transform, false);
        RectTransform p1Rect = p1Obj.GetComponent<RectTransform>();
        p1Rect.anchorMin = new Vector2(0, 1);
        p1Rect.anchorMax = new Vector2(0, 1);
        p1Rect.anchoredPosition = new Vector2(100, -100);
        p1Rect.sizeDelta = new Vector2(150, 50);
        p1MovesText = p1Obj.GetComponent<Text>();
        p1MovesText.text = "P1 (X): 0";
        p1MovesText.fontSize = 24;
        p1MovesText.alignment = TextAnchor.MiddleLeft;
        p1MovesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // Player 2 moves
        GameObject p2Obj = new GameObject("P2MovesText", typeof(RectTransform), typeof(Text));
        p2Obj.transform.SetParent(canvas.transform, false);
        RectTransform p2Rect = p2Obj.GetComponent<RectTransform>();
        p2Rect.anchorMin = new Vector2(0, 1);
        p2Rect.anchorMax = new Vector2(0, 1);
        p2Rect.anchoredPosition = new Vector2(100, -160);
        p2Rect.sizeDelta = new Vector2(150, 50);
        p2MovesText = p2Obj.GetComponent<Text>();
        p2MovesText.text = "P2 (O): 0";
        p2MovesText.fontSize = 24;
        p2MovesText.alignment = TextAnchor.MiddleLeft;
        p2MovesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
}