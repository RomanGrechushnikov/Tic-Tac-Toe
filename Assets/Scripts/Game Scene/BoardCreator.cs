using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public Button[] cells { get; private set; }
    public Text[] cellTexts { get; private set; }
    
    public void CreateBoard(Canvas canvas, float cellSize = 100f, float spacing = 10f)
    {
        cells = new Button[9];
        cellTexts = new Text[9];
        
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
            
            cellTexts[i] = text;
            cells[i] = cell.GetComponent<Button>();
        }
    }
    
    public void SetMark(int index, string mark)
    {
        cellTexts[index].text = mark;
    }
    
    public void ResetBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            cellTexts[i].text = "";
        }
    }
}