using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject exitPopup;
    
    public void ShowExitPopup()
    {
        exitPopup.SetActive(true);
    }
    
    public void HideExitPopup()
    {
        AudioManager.Instance.PlayPopupClose();
        exitPopup.SetActive(false);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
