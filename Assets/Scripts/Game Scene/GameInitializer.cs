using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameInitializer : MonoBehaviour
{
    private float startTime;
    private int p1Moves = 0;
    private int p2Moves = 0;
    private string currentPlayer = "X";
    private bool gameActive = true;
    
    private BoardCreator board;
    private GameUIManager ui;
    private ThemeLoader theme;
    private GameOverPopup gameOverPopup;

    void Start()
    {
        startTime = Time.time;
        CreateCanvas();
        CreateEventSystem();
        
        board = gameObject.AddComponent<BoardCreator>();
        ui = gameObject.AddComponent<GameUIManager>();
        theme = gameObject.AddComponent<ThemeLoader>();
        theme.LoadTheme();
        Canvas canvas = FindAnyObjectByType<Canvas>();
        
        board.CreateBoard(canvas);
        ui.CreateHUD(canvas);
        
        if (board.cells != null)
        {
            for (int i = 0; i < 9; i++)
            {
                int idx = i;
                board.cells[idx].onClick.AddListener(() => OnCellClick(idx));
            }
        }
    }

    void OnCellClick(int index)
    {
        if (!gameActive || board.hiddenTexts[index].text != "") return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);

        Sprite moveSprite = (currentPlayer == "X") ? theme.xSprite : theme.oSprite;
        board.SetMark(index, currentPlayer, moveSprite);

        if (currentPlayer == "X") p1Moves++; else p2Moves++;

        // Update HUD safely
        if (ui.p1MovesText != null) ui.p1MovesText.text = $"P1 (X): {p1Moves}";
        if (ui.p2MovesText != null) ui.p2MovesText.text = $"P2 (O): {p2Moves}";

        int[] winningPattern = GetWinningPattern();
        if (winningPattern != null)
        {
            gameActive = false;
            float duration = Time.time - startTime;
            WinAnimation.AnimateWin(board.cells, winningPattern);
            GameStatsSaver.SaveGameResult(currentPlayer, duration); // Using central saver
            Invoke("DelayedGameOver", 1.5f);
            return;
        }
        
        if (p1Moves + p2Moves >= 9)
        {
            gameActive = false;
            float duration = Time.time - startTime;
            GameStatsSaver.SaveGameResult("Draw", duration);
            ShowGameOver("Draw", duration);
            return;
        }
        
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
    }

    void DelayedGameOver()
    {
        ShowGameOver(currentPlayer, Time.time - startTime);
    }

    void ShowGameOver(string winner, float duration)
    {
        if (gameOverPopup == null)
            gameOverPopup = gameObject.AddComponent<GameOverPopup>();
        
        gameOverPopup.Create(FindAnyObjectByType<Canvas>());
        gameOverPopup.ShowGameOver(winner, duration);
    }

    int[] GetWinningPattern()
    {
        string[] boardState = new string[9];
        for (int i = 0; i < 9; i++)
            boardState[i] = board.hiddenTexts[i].text;
        
        int[][] patterns = new int[][]
        {
            new int[] {0,1,2}, new int[] {3,4,5}, new int[] {6,7,8},
            new int[] {0,3,6}, new int[] {1,4,7}, new int[] {2,5,8},
            new int[] {0,4,8}, new int[] {2,4,6}
        };
        
        foreach (var p in patterns)
        {
            if (boardState[p[0]] != "" && 
                boardState[p[0]] == boardState[p[1]] && 
                boardState[p[1]] == boardState[p[2]])
                return p;
        }
        return null;
    }

    void CreateCanvas()
    {
        if (FindAnyObjectByType<Canvas>() != null) return;
        GameObject canvasObj = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasObj.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
    }

    void CreateEventSystem()
    {
        if (FindAnyObjectByType<EventSystem>() != null) return;
        new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));
    }

    void Update()
    {
        if (gameActive && ui != null && ui.timerText != null)
        {
            ui.timerText.text = "Time: " + (Time.time - startTime).ToString("F1");
        }
    }
}