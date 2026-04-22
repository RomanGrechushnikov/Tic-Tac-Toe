using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleSound : MonoBehaviour
{
    private Toggle toggle;
    
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }
    
    void OnToggleChanged(bool value)
    {
        AudioManager.Instance.PlayToggle();
    }
}