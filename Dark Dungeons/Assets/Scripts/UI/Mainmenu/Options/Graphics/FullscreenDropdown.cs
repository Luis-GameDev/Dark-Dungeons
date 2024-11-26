using UnityEngine;
using TMPro;  

public class ScreenModeManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int currentScreenMode = 0;

    void Start()
    {
        PopulateDropdown();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);

        ApplyScreenMode(currentScreenMode);
    }

    private void PopulateDropdown()
    {
        resolutionDropdown.ClearOptions();
        
        var options = new System.Collections.Generic.List<string>
        {
            "Windowed",          // Option 0
            "Full Screen (Windowed)", // Option 1
            "Full Screen (Exclusive)"  // Option 2
        };
        
        resolutionDropdown.AddOptions(options);

        resolutionDropdown.value = currentScreenMode;
    }

    private void OnResolutionChanged(int index)
    {
        currentScreenMode = index;
        ApplyScreenMode(currentScreenMode);
    }

    private void ApplyScreenMode(int screenMode)
    {
        switch (screenMode)
        {
            case 0:
                // Windowed Mode
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, false);  
                break;

            case 1:
                // Full Screen Windowed
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); 
                break;

            case 2:
                // Exclusive Full Screen
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true); 
                break;
        }
    }
}
