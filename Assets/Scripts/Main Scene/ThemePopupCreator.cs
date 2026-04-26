using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Added for style consistency

public class ThemePopupCreator : MonoBehaviour
{
    public static Sprite SelectedXSprite; 
    public static Sprite SelectedOSprite;  
    [Header("Drag X and O sprites here")]
    public Sprite theme1_X;
    public Sprite theme1_O;
    public Sprite theme2_X;
    public Sprite theme2_O;
    public Sprite theme3_X;
    public Sprite theme3_O;
    
    private GameObject popup;
    private Button startButton;
    private int selectedTheme = -1;
    private GameObject[] highlights = new GameObject[3];
    
    void CreatePopup()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;
        
        popup = new GameObject("PopupTheme");
        popup.transform.SetParent(canvas.transform, false);
        RectTransform popupRect = popup.AddComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.sizeDelta = Vector2.zero;
        
        Image popupImage = popup.AddComponent<Image>();
        popupImage.color = new Color(0, 0, 0, 0.7f); // Darker overlay to match ExitPopup
        
        GameObject panel = new GameObject("ThemePanel");
        panel.transform.SetParent(popup.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(900, 600);
        panelRect.anchoredPosition = Vector2.zero;
        
        Image panelImage = panel.AddComponent<Image>();
        // Using the dark blue-ish grey from StatsPopup
        panelImage.color = new Color(0.12f, 0.12f, 0.15f, 1f); 
        
        // Switched Title to TextMeshPro for sharp visuals
        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(TextMeshProUGUI));
        titleObj.transform.SetParent(panel.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(400, 80);
        titleRect.anchoredPosition = new Vector2(0, 230);
        
        TextMeshProUGUI titleText = titleObj.GetComponent<TextMeshProUGUI>();
        titleText.text = "CHOOSE THEME";
        titleText.fontSize = 48;
        titleText.alignment = TextAlignmentOptions.Center;
        titleText.color = Color.yellow; // Yellow title to match StatsPopup
        
        if (theme1_X != null && theme1_O != null)
            CreateTheme(panel.transform, "Theme1", new Vector2(-280, 20), 0, theme1_X, theme1_O);
        
        if (theme2_X != null && theme2_O != null)
            CreateTheme(panel.transform, "Theme2", new Vector2(0, 20), 1, theme2_X, theme2_O);
        
        if (theme3_X != null && theme3_O != null)
            CreateTheme(panel.transform, "Theme3", new Vector2(280, 20), 2, theme3_X, theme3_O);
        
        CreateStartButton(panel.transform);
    }
    
    void CreateTheme(Transform parent, string name, Vector2 position, int index, Sprite xSprite, Sprite oSprite)
    {
        GameObject theme = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        theme.transform.SetParent(parent, false);
        
        RectTransform rect = theme.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(220, 220); // Slightly larger cards
        rect.anchoredPosition = position;
        
        Image bg = theme.GetComponent<Image>();
        bg.color = new Color(0.2f, 0.2f, 0.25f, 1f); // Darker card background
        
        GameObject highlight = new GameObject("Highlight", typeof(RectTransform), typeof(Image));
        highlight.transform.SetParent(theme.transform, false);
        RectTransform highRect = highlight.GetComponent<RectTransform>();
        highRect.anchorMin = Vector2.zero;
        highRect.anchorMax = Vector2.one;
        highRect.sizeDelta = new Vector2(10, 10); // Thicker selection border
        Image highImg = highlight.GetComponent<Image>();
        highImg.color = Color.green; // Changed yellow highlight to green for "selection" feel
        highlight.SetActive(false);
        highlights[index] = highlight;
        
        // Preview Icons
        CreatePreviewIcon(theme.transform, xSprite, new Vector2(-50, 0));
        CreatePreviewIcon(theme.transform, oSprite, new Vector2(50, 0));
        
        Button btn = theme.GetComponent<Button>();
        int idx = index;
        btn.onClick.AddListener(() => SelectTheme(idx));
    }

    void CreatePreviewIcon(Transform parent, Sprite sprite, Vector2 pos)
    {
        GameObject imgObj = new GameObject("Icon", typeof(RectTransform), typeof(Image));
        imgObj.transform.SetParent(parent, false);
        RectTransform rect = imgObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(80, 80);
        rect.anchoredPosition = pos;
        imgObj.GetComponent<Image>().sprite = sprite;
    }
    
    void CreateStartButton(Transform parent)
    {
        GameObject startBtn = new GameObject("StartBtn", typeof(RectTransform), typeof(Image), typeof(Button));
        startBtn.transform.SetParent(parent, false);
        
        RectTransform rect = startBtn.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(300, 70); // Wider button
        rect.anchoredPosition = new Vector2(0, -230);
        
        Image btnImage = startBtn.GetComponent<Image>();
        btnImage.color = new Color(0.4f, 0.4f, 0.4f); // Grey default
        
        startButton = startBtn.GetComponent<Button>();
        startButton.interactable = false;
        startButton.onClick.AddListener(() => {
            PlayerPrefs.SetInt("SelectedTheme", selectedTheme);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameScene");
        });
        
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(startBtn.transform, false);
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = rect.sizeDelta;
        
        TextMeshProUGUI text = textObj.GetComponent<TextMeshProUGUI>();
        text.text = "START GAME";
        text.fontSize = 32;
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
    }
    
    void SelectTheme(int theme)
    {
        selectedTheme = theme;
        PlayerPrefs.SetInt("SelectedTheme", theme);
        
        // Logical mapping stays same as requested
        if (theme == 0) { SelectedXSprite = theme1_X; SelectedOSprite = theme1_O; }
        else if (theme == 1) { SelectedXSprite = theme2_X; SelectedOSprite = theme2_O; }
        else if (theme == 2) { SelectedXSprite = theme3_X; SelectedOSprite = theme3_O; }
        
        for (int i = 0; i < highlights.Length; i++)
            if (highlights[i] != null) highlights[i].SetActive(i == theme);
        
        if (startButton != null)
        {
            startButton.interactable = true;
            startButton.GetComponent<Image>().color = new Color(0.1f, 0.8f, 0.1f); // Success Green
        }
    }

    public void ShowPopup()
    {
        if (popup == null)
            CreatePopup();
        popup.SetActive(true);
    }
}