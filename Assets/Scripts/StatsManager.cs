using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{
    public GameObject statsPopup;
    public Text totalGamesText;
    public Text p1WinsText;
    public Text p2WinsText;
    public Text drawsText;
    public Text avgDurationText;
    
    void Start()
    {
        statsPopup.SetActive(false);
    }
    
    public void ShowStats()
    {
        UpdateDisplay();
        statsPopup.SetActive(true);
    }
    
    public void HideStats()
    {
        AudioManager.Instance.PlayPopupClose();
        statsPopup.SetActive(false);
    }
    
    void UpdateDisplay()
    {
        int total = PlayerPrefs.GetInt("TotalGames", 0);
        int p1Wins = PlayerPrefs.GetInt("P1Wins", 0);
        int p2Wins = PlayerPrefs.GetInt("P2Wins", 0);
        int draws = PlayerPrefs.GetInt("Draws", 0);
        float totalDuration = PlayerPrefs.GetFloat("TotalDuration", 0);
        
        float avg = total > 0 ? totalDuration / total : 0;
        
        totalGamesText.text = $"Total Games: {total}";
        p1WinsText.text = $"Player 1 Wins: {p1Wins}";
        p2WinsText.text = $"Player 2 Wins: {p2Wins}";
        drawsText.text = $"Draws: {draws}";
        avgDurationText.text = $"Average Duration: {avg:F1}s";
    }
}