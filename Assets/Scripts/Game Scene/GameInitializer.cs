using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        CreateCanvas();
        CreateEventSystem();
        CreateBoard();
        CreateHUD();
    }
    
    void CreateCanvas()
    {
        GameObject canvas = new GameObject("Canvas");
        Canvas canvasComp = canvas.AddComponent<Canvas>();
        canvasComp.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();
    }
    
    void CreateEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() != null) return;
        
        GameObject es = new GameObject("EventSystem");
        es.AddComponent<EventSystem>();
        
        // Use InputSystemUIInputModule instead of StandaloneInputModule
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
    }
    
    void CreateBoard()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        float cellSize = 100f;
        float spacing = 10f;
        
        for (int i = 0; i < 9; i++)
        {
            GameObject cell = new GameObject($"Cell_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
            cell.transform.SetParent(canvas.transform, false);
            
            RectTransform rect = cell.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(cellSize, cellSize);
            
            int row = i / 3;
            int col = i % 3;
            float x = (col - 1) * (cellSize + spacing);
            float y = (1 - row) * (cellSize + spacing);
            rect.anchoredPosition = new Vector2(x, y);
            
            // Add text to cell
            GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
            textObj.transform.SetParent(cell.transform, false);
            
            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
            
            Text text = textObj.GetComponent<Text>();
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        }
    }
    
    void CreateHUD()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        
        // Timer text
        GameObject timerObj = new GameObject("TimerText", typeof(RectTransform), typeof(Text));
        timerObj.transform.SetParent(canvas.transform, false);
        
        RectTransform timerRect = timerObj.GetComponent<RectTransform>();
        timerRect.anchorMin = new Vector2(1, 1);
        timerRect.anchorMax = new Vector2(1, 1);
        timerRect.anchoredPosition = new Vector2(-100, -50);
        timerRect.sizeDelta = new Vector2(200, 50);
        
        Text timerText = timerObj.GetComponent<Text>();
        timerText.text = "Time: 0";
        timerText.fontSize = 24;
        timerText.alignment = TextAnchor.MiddleRight;
        timerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        // Move count text
        GameObject movesObj = new GameObject("MovesText", typeof(RectTransform), typeof(Text));
        movesObj.transform.SetParent(canvas.transform, false);
        
        RectTransform movesRect = movesObj.GetComponent<RectTransform>();
        movesRect.anchorMin = new Vector2(0, 1);
        movesRect.anchorMax = new Vector2(0, 1);
        movesRect.anchoredPosition = new Vector2(100, -50);
        movesRect.sizeDelta = new Vector2(200, 50);
        
        Text movesText = movesObj.GetComponent<Text>();
        movesText.text = "Moves: 0";
        movesText.fontSize = 24;
        movesText.alignment = TextAnchor.MiddleLeft;
        movesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
}