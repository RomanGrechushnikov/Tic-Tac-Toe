using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsPopupGenerator : MonoBehaviour
{
    private GameObject popup;

    public void CreateStatsPopup()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        // 1. Root & Overlay
        popup = new GameObject("PopupStats");
        popup.transform.SetParent(canvas.transform, false);
        RectTransform rootRect = popup.AddComponent<RectTransform>();
        rootRect.anchorMin = Vector2.zero;
        rootRect.anchorMax = Vector2.one;
        rootRect.sizeDelta = Vector2.zero;

        GameObject overlay = new GameObject("Overlay");
        overlay.transform.SetParent(popup.transform, false);
        overlay.AddComponent<RectTransform>().anchorMin = Vector2.zero;
        overlay.GetComponent<RectTransform>().anchorMax = Vector2.one;
        overlay.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        overlay.AddComponent<Image>().color = new Color(0, 0, 0, 0.6f);

        // 2. Panel
        GameObject panel = new GameObject("Panel");
        panel.transform.SetParent(popup.transform, false);
        RectTransform panelRect = panel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(450, 550);
        panel.AddComponent<Image>().color = new Color(0.12f, 0.12f, 0.15f); // Dark blue-ish grey

        // 3. Title
        CreateText(panel.transform, "STATISTICS", new Vector2(0, 220), 36, Color.yellow);

        // 4. Fetch and Display Data
        int total = PlayerPrefs.GetInt("TotalGames", 0);
        int p1Wins = PlayerPrefs.GetInt("P1Wins", 0);
        int p2Wins = PlayerPrefs.GetInt("P2Wins", 0);
        int draws = PlayerPrefs.GetInt("Draws", 0);
        float totalDuration = PlayerPrefs.GetFloat("TotalDuration", 0);
        float avg = total > 0 ? totalDuration / total : 0;

        float startY = 120f;
        float spacing = 50f;

        CreateStatLine(panel.transform, $"Total Games: {total}", new Vector2(0, startY));
        CreateStatLine(panel.transform, $"Player 1 Wins: {p1Wins}", new Vector2(0, startY - spacing));
        CreateStatLine(panel.transform, $"Player 2 Wins: {p2Wins}", new Vector2(0, startY - (spacing * 2)));
        CreateStatLine(panel.transform, $"Draws: {draws}", new Vector2(0, startY - (spacing * 3)));
        CreateStatLine(panel.transform, $"Avg. Match: {avg:F1}s", new Vector2(0, startY - (spacing * 4)));

        // 5. Close Button
        GameObject closeBtnObj = new GameObject("Btn_Close", typeof(RectTransform), typeof(Image), typeof(Button));
        closeBtnObj.transform.SetParent(panel.transform, false);
        RectTransform btnRect = closeBtnObj.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(180, 55);
        btnRect.anchoredPosition = new Vector2(0, -220);
        closeBtnObj.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
        
        CreateText(closeBtnObj.transform, "BACK", Vector2.zero, 26, Color.white);
        
        closeBtnObj.GetComponent<Button>().onClick.AddListener(() => {
            if (AudioManager.Instance != null) AudioManager.Instance.PlayPopupClose();
            Destroy(popup);
        });
    }

    void CreateStatLine(Transform parent, string content, Vector2 pos)
    {
        CreateText(parent, content, pos, 24, Color.white);
    }

    void CreateText(Transform parent, string content, Vector2 pos, int size, Color color)
    {
        GameObject txtObj = new GameObject("Text", typeof(TextMeshProUGUI));
        txtObj.transform.SetParent(parent, false);
        var tmp = txtObj.GetComponent<TextMeshProUGUI>();
        tmp.text = content;
        tmp.fontSize = size;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.GetComponent<RectTransform>().anchoredPosition = pos;
        tmp.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 50);
    }
}