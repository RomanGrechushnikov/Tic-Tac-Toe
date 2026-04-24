using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInitializer : MonoBehaviour
{
    private float startTime;
    private int p1Moves = 0;
    private int p2Moves = 0;
    private string currentPlayer = "X";
    private bool gameActive = true;
    
    private BoardCreator board;
    private GameUIManager ui;
    private GameOverPopup gameOverPopup;
    
    void Start()
    {
        startTime = Time.time;
        
        CreateCanvas();
        CreateEventSystem();
        
        board = gameObject.AddComponent<BoardCreator>();
        ui = gameObject.AddComponent<GameUIManager>();
        
        Canvas canvas = FindAnyObjectByType<Canvas>();
        board.CreateBoard(canvas);
        ui.CreateHUD(canvas);
        
        for (int i = 0; i < 9; i++)
        {
            int idx = i;
            board.cells[i].onClick.AddListener(() => OnCellClick(idx));
        }
    }
    
    void Update()
    {
        if (ui.timerText != null)
        {
            float elapsed = Time.time - startTime;
            ui.timerText.text = "Time: " + elapsed.ToString("F1");
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
    
    void OnCellClick(int index)
    {
        if (!gameActive) return;
        if (board.cellTexts[index].text != "") return;
        
        board.SetMark(index, currentPlayer);
        
        if (currentPlayer == "X")
        {
            p1Moves++;
            ui.p1MovesText.text = $"P1 (X): {p1Moves}";
        }
        else
        {
            p2Moves++;
            ui.p2MovesText.text = $"P2 (O): {p2Moves}";
        }
        
        if (CheckWin())
        {
            gameActive = false;
            float duration = Time.time - startTime;
            ShowGameOver(currentPlayer, duration);
            return;
        }
        
        if (p1Moves + p2Moves >= 9)
        {
            gameActive = false;
            float duration = Time.time - startTime;
            ShowGameOver("Draw", duration);
            return;
        }
        
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
    }
    
    bool CheckWin()
    {
        string[] boardState = new string[9];
        for (int i = 0; i < 9; i++)
            boardState[i] = board.cellTexts[i].text;
        
        int[][] patterns = new int[][]
        {
            new int[] {0,1,2}, new int[] {3,4,5}, new int[] {6,7,8},
            new int[] {0,3,6}, new int[] {1,4,7}, new int[] {2,5,8},
            new int[] {0,4,8}, new int[] {2,4,6}
        };
        
        foreach (var p in patterns)
        {
            if (boardState[p[0]] != "" && boardState[p[0]] == boardState[p[1]] && boardState[p[1]] == boardState[p[2]])
                return true;
        }
        return false;
    }
    
    void ShowGameOver(string winner, float duration)
    {
        gameActive = false;
        
        if (gameOverPopup == null)
        {
            Canvas canvas = FindAnyObjectByType<Canvas>();
            gameOverPopup = new GameObject("GameOverPopupManager").AddComponent<GameOverPopup>();
            gameOverPopup.Create(canvas);
        }
        
        gameOverPopup.ShowGameOver(winner, duration);
    }
}