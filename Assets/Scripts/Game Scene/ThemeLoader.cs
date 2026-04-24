using UnityEngine;

public class ThemeLoader : MonoBehaviour
{
    public Sprite xSprite { get; private set; }
    public Sprite oSprite { get; private set; }
    
    public void LoadTheme()
    {
        xSprite = ThemePopupCreator.SelectedXSprite;
        oSprite = ThemePopupCreator.SelectedOSprite;
        
        if (xSprite == null || oSprite == null)
        {
            Debug.LogError("Theme not selected! Using defaults.");
            Sprite[] all = Resources.FindObjectsOfTypeAll<Sprite>();
            foreach (Sprite s in all)
            {
                if (s.name == "XO_0") xSprite = s;
                if (s.name == "XO_1") oSprite = s;
            }
        }
        else
        {
            Debug.Log($"Theme loaded: X={xSprite.name}, O={oSprite.name}");
        }
    }
}