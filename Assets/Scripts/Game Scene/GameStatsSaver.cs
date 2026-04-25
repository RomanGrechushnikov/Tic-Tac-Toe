using UnityEngine;

public static class GameStatsSaver
{
    public static void SaveGameResult(string winner, float duration)
    {
        // 1. Update Total Games
        int totalGames = PlayerPrefs.GetInt("TotalGames", 0);
        totalGames++;
        PlayerPrefs.SetInt("TotalGames", totalGames);

        // 2. Update Wins / Draws
        if (winner == "X")
        {
            int p1Wins = PlayerPrefs.GetInt("P1Wins", 0);
            PlayerPrefs.SetInt("P1Wins", p1Wins + 1);
        }
        else if (winner == "O")
        {
            int p2Wins = PlayerPrefs.GetInt("P2Wins", 0);
            PlayerPrefs.SetInt("P2Wins", p2Wins + 1);
        }
        else // It's a Draw
        {
            int draws = PlayerPrefs.GetInt("Draws", 0);
            PlayerPrefs.SetInt("Draws", draws + 1);
        }

        // 3. Update Total Duration (for Average calculation)
        float totalDuration = PlayerPrefs.GetFloat("TotalDuration", 0f);
        totalDuration += duration;
        PlayerPrefs.SetFloat("TotalDuration", totalDuration);

        // 4. Force Save to Disk (Important for WebGL)
        PlayerPrefs.Save();
        
        Debug.Log($"Stats Saved! Total Games: {totalGames}, Winner: {winner}, Duration: {duration}s");
    }
}