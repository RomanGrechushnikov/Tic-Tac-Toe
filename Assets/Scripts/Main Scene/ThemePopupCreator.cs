using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        popupImage.color = new Color(0, 0, 0, 0.6f);
        
        GameObject panel = new GameObject("ThemePanel");
        panel.transform.SetParent(popup.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(900, 600);
        panelRect.anchoredPosition = Vector2.zero;
        
        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = Color.white;
        
        GameObject titleObj = new GameObject("Title", typeof(RectTransform), typeof(Text));
        titleObj.transform.SetParent(panel.transform, false);
        RectTransform titleRect = titleObj.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.5f);
        titleRect.anchorMax = new Vector2(0.5f, 0.5f);
        titleRect.sizeDelta = new Vector2(400, 80);
        titleRect.anchoredPosition = new Vector2(0, 220);
        
        Text titleText = titleObj.GetComponent<Text>();
        titleText.text = "CHOOSE THEME";
        titleText.fontSize = 48;
        titleText.alignment = TextAnchor.MiddleCenter;
        Font titleFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (titleFont != null) titleText.font = titleFont;
        titleText.color = Color.black;
        
        if (theme1_X != null && theme1_O != null)
            CreateTheme(panel.transform, "Theme1", new Vector2(-280, 0), 0, theme1_X, theme1_O);
        
        if (theme2_X != null && theme2_O != null)
            CreateTheme(panel.transform, "Theme2", new Vector2(0, 0), 1, theme2_X, theme2_O);
        
        if (theme3_X != null && theme3_O != null)
            CreateTheme(panel.transform, "Theme3", new Vector2(280, 0), 2, theme3_X, theme3_O);
        
        CreateStartButton(panel.transform);
    }
    
    void CreateTheme(Transform parent, string name, Vector2 position, int index, Sprite xSprite, Sprite oSprite)
    {
        GameObject theme = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        theme.transform.SetParent(parent, false);
        
        RectTransform rect = theme.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(180, 180);
        rect.anchoredPosition = position;
        
        Image bg = theme.GetComponent<Image>();
        bg.color = new Color(0.9f, 0.9f, 0.9f);
        
        GameObject highlight = new GameObject("Highlight", typeof(RectTransform), typeof(Image));
        highlight.transform.SetParent(theme.transform, false);
        RectTransform highRect = highlight.GetComponent<RectTransform>();
        highRect.anchorMin = Vector2.zero;
        highRect.anchorMax = Vector2.one;
        highRect.sizeDelta = Vector2.zero;
        Image highImg = highlight.GetComponent<Image>();
        highImg.color = Color.yellow;
        highlight.SetActive(false);
        highlights[index] = highlight;
        
        GameObject xImg = new GameObject("X_Image", typeof(RectTransform), typeof(Image));
        xImg.transform.SetParent(theme.transform, false);
        RectTransform xRect = xImg.GetComponent<RectTransform>();
        xRect.anchorMin = new Vector2(0.5f, 0.5f);
        xRect.anchorMax = new Vector2(0.5f, 0.5f);
        xRect.sizeDelta = new Vector2(70, 70);
        xRect.anchoredPosition = new Vector2(-45, 0);
        Image xImage = xImg.GetComponent<Image>();
        xImage.sprite = xSprite;
        
        GameObject oImg = new GameObject("O_Image", typeof(RectTransform), typeof(Image));
        oImg.transform.SetParent(theme.transform, false);
        RectTransform oRect = oImg.GetComponent<RectTransform>();
        oRect.anchorMin = new Vector2(0.5f, 0.5f);
        oRect.anchorMax = new Vector2(0.5f, 0.5f);
        oRect.sizeDelta = new Vector2(70, 70);
        oRect.anchoredPosition = new Vector2(45, 0);
        Image oImage = oImg.GetComponent<Image>();
        oImage.sprite = oSprite;
        
        Button btn = theme.GetComponent<Button>();
        int idx = index;
        btn.onClick.AddListener(() => SelectTheme(idx));
    }
    
    void CreateStartButton(Transform parent)
    {
        GameObject startBtn = new GameObject("StartBtn", typeof(RectTransform), typeof(Image), typeof(Button));
        startBtn.transform.SetParent(parent, false);
        
        RectTransform rect = startBtn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(250, 60);
        rect.anchoredPosition = new Vector2(0, -220);
        
        Image btnImage = startBtn.GetComponent<Image>();
        btnImage.color = Color.gray;
        
        startButton = startBtn.GetComponent<Button>();
        startButton.interactable = false;
        startButton.onClick.AddListener(() => {
            Debug.Log("Theme selected: " + selectedTheme);
            PlayerPrefs.SetInt("SelectedTheme", selectedTheme);
            PlayerPrefs.Save();
            SceneManager.LoadScene("GameScene");
        });
        
        GameObject textObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textObj.transform.SetParent(startBtn.transform, false);
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        Text text = textObj.GetComponent<Text>();
        text.text = "START";
        text.fontSize = 32;
        text.alignment = TextAnchor.MiddleCenter;
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (font != null) text.font = font;
        text.color = Color.white;
    }
    
    void SelectTheme(int theme)
    {
        selectedTheme = theme;
        PlayerPrefs.SetInt("SelectedTheme", theme);
        
        if (theme == 0)
        {
            SelectedXSprite = theme1_X;
            SelectedOSprite = theme1_O;
        }
        else if (theme == 1)
        {
            SelectedXSprite = theme2_X;
            SelectedOSprite = theme2_O;
        }
        else if (theme == 2)
        {
            SelectedXSprite = theme3_X;
            SelectedOSprite = theme3_O;
        }
        
        for (int i = 0; i < highlights.Length; i++)
        {
            if (highlights[i] != null)
                highlights[i].SetActive(i == theme);
        }
        
        if (startButton != null)
        {
            startButton.interactable = true;
            startButton.GetComponent<Image>().color = Color.green;
        }
    }

    public void ShowPopup()
    {
        if (popup == null)
            CreatePopup();
        popup.SetActive(true);
    }
}