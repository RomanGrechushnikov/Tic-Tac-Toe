using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInitializer : MonoBehaviour
{
    private float startTime;
    private Text timerText;
    private Text p1MovesText;
    private Text p2MovesText;
    private int p1Moves = 0;
    private int p2Moves = 0;
    private Button[] cells;
    private string currentPlayer = "X";
    
    void Start()
    {
        startTime = Time.time;
        CreateCanvas();
        CreateEventSystem();
        CreateBoard();
        CreateHUD();
    }
    
    void Update()
    {
        if (timerText != null)
        {
            float elapsed = Time.time - startTime;
            timerText.text = "Time: " + elapsed.ToString("F1");
        }
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
        es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
    }
    
    void CreateBoard()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        float cellSize = 100f;
        float spacing = 10f;
        
        cells = new Button[9];
        
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
            
            Button btn = cell.GetComponent<Button>();
            cells[i] = btn;
            int index = i;
            btn.onClick.AddListener(() => OnCellClick(index));
        }
    }
    
    void CreateHUD()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        
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
        
        GameObject p1MovesObj = new GameObject("P1MovesText", typeof(RectTransform), typeof(Text));
        p1MovesObj.transform.SetParent(canvas.transform, false);
        
        RectTransform p1MovesRect = p1MovesObj.GetComponent<RectTransform>();
        p1MovesRect.anchorMin = new Vector2(0, 1);
        p1MovesRect.anchorMax = new Vector2(0, 1);
        p1MovesRect.anchoredPosition = new Vector2(100, -100);
        p1MovesRect.sizeDelta = new Vector2(150, 50);
        
        p1MovesText = p1MovesObj.GetComponent<Text>();
        p1MovesText.text = "P1 (X): 0";
        p1MovesText.fontSize = 24;
        p1MovesText.alignment = TextAnchor.MiddleLeft;
        p1MovesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        
        GameObject p2MovesObj = new GameObject("P2MovesText", typeof(RectTransform), typeof(Text));
        p2MovesObj.transform.SetParent(canvas.transform, false);
        
        RectTransform p2MovesRect = p2MovesObj.GetComponent<RectTransform>();
        p2MovesRect.anchorMin = new Vector2(0, 1);
        p2MovesRect.anchorMax = new Vector2(0, 1);
        p2MovesRect.anchoredPosition = new Vector2(100, -160);
        p2MovesRect.sizeDelta = new Vector2(150, 50);
        
        p2MovesText = p2MovesObj.GetComponent<Text>();
        p2MovesText.text = "P2 (O): 0";
        p2MovesText.fontSize = 24;
        p2MovesText.alignment = TextAnchor.MiddleLeft;
        p2MovesText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
    }
    
    void OnCellClick(int index)
    {
        Button btn = cells[index];
        Text text = btn.GetComponentInChildren<Text>();
        
        if (text.text != "") return;
        
        text.text = currentPlayer;
        
        if (currentPlayer == "X")
        {
            p1Moves++;
            p1MovesText.text = $"P1 (X): {p1Moves}";
        }
        else
        {
            p2Moves++;
            p2MovesText.text = $"P2 (O): {p2Moves}";
        }
        
        if (CheckWin())
        {
            Debug.Log(currentPlayer + " wins!");
            return;
        }
        
        int totalMoves = p1Moves + p2Moves;
        if (totalMoves >= 9)
        {
            Debug.Log("Draw!");
            return;
        }
        
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
    }
    
    bool CheckWin()
    {
        string[] board = new string[9];
        for (int i = 0; i < 9; i++)
        {
            Text t = cells[i].GetComponentInChildren<Text>();
            board[i] = t.text;
        }
        
        int[][] patterns = new int[][]
        {
            new int[] {0,1,2}, new int[] {3,4,5}, new int[] {6,7,8},
            new int[] {0,3,6}, new int[] {1,4,7}, new int[] {2,5,8},
            new int[] {0,4,8}, new int[] {2,4,6}
        };
        
        foreach (var p in patterns)
        {
            if (board[p[0]] != "" && board[p[0]] == board[p[1]] && board[p[1]] == board[p[2]])
                return true;
        }
        return false;
    }
}