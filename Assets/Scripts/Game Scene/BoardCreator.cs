using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public Button[] cells { get; private set; }
    public Image[] markImages { get; private set; }
    public Text[] hiddenTexts { get; private set; }
    
    // Default size updated to 0.9f as requested
    public void CreateBoard(Canvas canvas, float boardSizePercent = 0.8f, float spacingPercent = 0.02f)
    {
        cells = new Button[9];
        markImages = new Image[9];
        hiddenTexts = new Text[9];

        // 1. Setup the Parent Container
        GameObject boardObj = new GameObject("GameBoard", typeof(RectTransform), typeof(Image));
        boardObj.transform.SetParent(canvas.transform, false);
        
        RectTransform boardRect = boardObj.GetComponent<RectTransform>();
        boardRect.anchorMin = new Vector2(0.5f, 0.5f);
        boardRect.anchorMax = new Vector2(0.5f, 0.5f);
        boardRect.pivot = new Vector2(0.5f, 0.5f);

        // --- NEW RELATIVE SCALING LOGIC ---
        float boardSize;
        if (Screen.height > Screen.width)
        {
            // Portrait mode: base size on width
            boardSize = Screen.width * boardSizePercent;
        }
        else
        {
            // Landscape mode: base size on height
            boardSize = Screen.height * boardSizePercent;
        }
        // ----------------------------------

        boardRect.sizeDelta = new Vector2(boardSize, boardSize);
        boardObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);

        // 2. Grid Layout Group
        GridLayoutGroup grid = boardObj.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
        
        // Relative Padding (approx 2% of board size)
        int paddingValue = Mathf.RoundToInt(boardSize * 0.02f);
        grid.padding = new RectOffset(paddingValue, paddingValue, paddingValue, paddingValue);
        
        float spacing = boardSize * spacingPercent;
        grid.spacing = new Vector2(spacing, spacing);
        
        float totalPadding = grid.padding.left + grid.padding.right;
        float totalSpacing = spacing * 2;
        float calculatedCellSize = (boardSize - totalPadding - totalSpacing) / 3;
        grid.cellSize = new Vector2(calculatedCellSize, calculatedCellSize);

        // 3. Generate Cells
        for (int i = 0; i < 9; i++)
        {
            GameObject cell = new GameObject($"Cell_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
            cell.transform.SetParent(boardObj.transform, false);
            
            // Marks (X or O)
            GameObject markObj = new GameObject("Mark", typeof(RectTransform), typeof(Image));
            markObj.transform.SetParent(cell.transform, false);
            RectTransform mRect = markObj.GetComponent<RectTransform>();
            mRect.anchorMin = new Vector2(0.1f, 0.1f);
            mRect.anchorMax = new Vector2(0.9f, 0.9f);
            mRect.sizeDelta = Vector2.zero;
            
            markImages[i] = markObj.GetComponent<Image>();
            markImages[i].color = Color.clear;
            
            // Logic state storage
            GameObject hiddenObj = new GameObject("HiddenText", typeof(RectTransform), typeof(Text));
            hiddenObj.transform.SetParent(cell.transform, false);
            hiddenObj.SetActive(false);
            hiddenTexts[i] = hiddenObj.GetComponent<Text>();
            hiddenTexts[i].text = "";
            
            cells[i] = cell.GetComponent<Button>();
        }
    }

    public void SetMark(int index, string player, Sprite sprite)
    {
        markImages[index].sprite = sprite;
        markImages[index].color = Color.white;
        hiddenTexts[index].text = player;
    }
}