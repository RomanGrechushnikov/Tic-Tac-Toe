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
            GameObject cell = new GameObject($"Cell_{i}", typeof(RectTransform), typeof(Image), typeof(Button));
            cell.transform.SetParent(canvas.transform, false);
            
            RectTransform rect = cell.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(cellSize, cellSize);
            
            int row = i / 3;
            int col = i % 3;
            rect.anchoredPosition = new Vector2((col - 1) * (cellSize + spacing), (1 - row) * (cellSize + spacing));
            
            GameObject markObj = new GameObject("Mark", typeof(RectTransform), typeof(Image));
            markObj.transform.SetParent(cell.transform, false);
            markImages[i] = markObj.GetComponent<Image>();
            markImages[i].color = Color.clear;
            
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