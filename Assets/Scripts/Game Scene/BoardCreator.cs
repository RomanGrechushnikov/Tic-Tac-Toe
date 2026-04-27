using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public Button[] cells { get; private set; }
    public Image[] markImages { get; private set; }
    public Text[] hiddenTexts { get; private set; }
    
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

        // Relative scaling based on screen height
        float boardSize = Screen.height * boardSizePercent;
        boardRect.sizeDelta = new Vector2(boardSize, boardSize);
        boardObj.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);

        // 2. Grid Layout Group
        GridLayoutGroup grid = boardObj.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 3;
        grid.padding = new RectOffset(10, 10, 10, 10);
        
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
            
            GameObject markObj = new GameObject("Mark", typeof(RectTransform), typeof(Image));
            markObj.transform.SetParent(cell.transform, false);
            RectTransform mRect = markObj.GetComponent<RectTransform>();
            mRect.anchorMin = new Vector2(0.1f, 0.1f);
            mRect.anchorMax = new Vector2(0.9f, 0.9f);
            mRect.sizeDelta = Vector2.zero;
            
            markImages[i] = markObj.GetComponent<Image>();
            markImages[i].color = Color.clear;
            
            GameObject hiddenObj = new GameObject("HiddenText", typeof(RectTransform), typeof(Text));
            hiddenObj.transform.SetParent(cell.transform, false);
            hiddenObj.SetActive(false);
            hiddenTexts[i] = hiddenObj.GetComponent<Text>();
            hiddenTexts[i].text = ""; // Initialize empty string
            
            cells[i] = cell.GetComponent<Button>();
        }
    }

    // This is the method the error was complaining about!
    public void SetMark(int index, string player, Sprite sprite)
    {
        markImages[index].sprite = sprite;
        markImages[index].color = Color.white;
        hiddenTexts[index].text = player;
    }
}