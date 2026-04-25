using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    public Button[] cells { get; private set; }
    public Image[] markImages { get; private set; }
    public Text[] hiddenTexts { get; private set; }
    
    public void CreateBoard(Canvas canvas, float cellSize = 100f, float spacing = 10f)
    {
        cells = new Button[9];
        markImages = new Image[9];
        hiddenTexts = new Text[9];
        
        for (int i = 0; i < 9; i++)
        {
            // Create the individual cell
            GameObject cell = new GameObject($"Cell_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
            cell.transform.SetParent(canvas.transform, false);
            
            RectTransform rect = cell.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(cellSize, cellSize);
            
            // Grid math
            int row = i / 3;
            int col = i % 3;
            float x = (col - 1) * (cellSize + spacing);
            float y = (1 - row) * (cellSize + spacing);
            rect.anchoredPosition = new Vector2(x, y);
            
            // Visual image for X/O (The "Mark")
            GameObject markObj = new GameObject("Mark", typeof(RectTransform), typeof(Image));
            markObj.transform.SetParent(cell.transform, false);
            
            RectTransform markRect = markObj.GetComponent<RectTransform>();
            markRect.anchorMin = Vector2.zero;
            markRect.anchorMax = Vector2.one;
            markRect.sizeDelta = Vector2.zero;
            
            markImages[i] = markObj.GetComponent<Image>();
            markImages[i].color = Color.clear; // Transparent until played
            
            // Hidden text used by GameInitializer to check for wins
            GameObject hiddenObj = new GameObject("HiddenText", typeof(RectTransform), typeof(Text));
            hiddenObj.transform.SetParent(cell.transform, false);
            hiddenObj.SetActive(false); // We don't need to see the raw text
            
            RectTransform hiddenRect = hiddenObj.GetComponent<RectTransform>();
            hiddenRect.anchorMin = Vector2.zero;
            hiddenRect.anchorMax = Vector2.one;
            hiddenRect.sizeDelta = Vector2.zero;
            
            hiddenTexts[i] = hiddenObj.GetComponent<Text>();
            hiddenTexts[i].font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            
            // Store the button reference
            cells[i] = cell.GetComponent<Button>();
        }
    }
    
    public void SetMark(int index, string player, Sprite sprite)
    {
        if (index < 0 || index >= markImages.Length) return;

        markImages[index].sprite = sprite;
        markImages[index].color = Color.white;
        hiddenTexts[index].text = player;
    }
    
    public void ResetBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            markImages[i].sprite = null;
            markImages[i].color = Color.clear;
            hiddenTexts[i].text = "";
        }
    }
}