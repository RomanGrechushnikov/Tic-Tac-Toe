using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    public Text timerText { get; private set; }
    public Text p1MovesText { get; private set; }
    public Text p2MovesText { get; private set; }
    
    public void CreateHUD(Canvas canvas)
    {
        // 1. Create Timer
        timerText = CreateTextElement(canvas.transform, "Timer", "Time: 0.0", new Vector2(0, -50));

        // 2. Create P1 Move Count
        p1MovesText = CreateTextElement(canvas.transform, "P1Moves", "P1 (X): 0", new Vector2(-200, -50));

        // 3. Create P2 Move Count
        p2MovesText = CreateTextElement(canvas.transform, "P2Moves", "P2 (O): 0", new Vector2(200, -50));

        // 4. Create the Exit Button
        CreateExitButton(canvas);
    }

    Text CreateTextElement(Transform parent, string name, string initialText, Vector2 pos)
    {
        GameObject obj = new GameObject(name, typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(parent, false);
        
        Text t = obj.GetComponent<Text>();
        t.text = initialText;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = 24;
        t.alignment = TextAnchor.UpperCenter;
        t.color = Color.white;
        
        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1);
        rect.anchorMax = new Vector2(0.5f, 1);
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(200, 50);
        
        return t;
    }

    void CreateExitButton(Canvas canvas)
    {
        GameObject btnObj = new GameObject("ExitButton", typeof(RectTransform), typeof(Image), typeof(Button));
        btnObj.transform.SetParent(canvas.transform, false);
        
        RectTransform rect = btnObj.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(70, -40);
        rect.sizeDelta = new Vector2(80, 40);

        btnObj.GetComponent<Image>().color = new Color(0.8f, 0.2f, 0.2f, 1f);

        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(() => {
            if (AudioManager.Instance != null) AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonClick);
            SceneManager.LoadScene("SampleScene"); 
        });

        GameObject txtObj = new GameObject("Text", typeof(RectTransform), typeof(Text));
        txtObj.transform.SetParent(btnObj.transform, false);
        Text t = txtObj.GetComponent<Text>();
        t.text = "EXIT";
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.alignment = TextAnchor.MiddleCenter;
        t.color = Color.white;
        
        RectTransform tRect = txtObj.GetComponent<RectTransform>();
        tRect.anchorMin = Vector2.zero;
        tRect.anchorMax = Vector2.one;
        tRect.sizeDelta = Vector2.zero;
    }
}