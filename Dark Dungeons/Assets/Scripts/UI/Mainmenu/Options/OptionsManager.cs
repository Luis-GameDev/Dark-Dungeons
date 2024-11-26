using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle vSyncToggle;
    [SerializeField] private TMP_Dropdown textureQualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown fullscreenModeDropdown;  

    void Start()
    {
        LoadSettings();
    }

    public void OnSettingsClose()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetInt("VSync", vSyncToggle.isOn ? 1 : 0);

        PlayerPrefs.SetInt("TextureQuality", textureQualityDropdown.value);

        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);

        PlayerPrefs.SetInt("FullscreenMode", fullscreenModeDropdown.value);

        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VSync"))
        {
            bool isVSyncEnabled = PlayerPrefs.GetInt("VSync") == 1;
            vSyncToggle.isOn = isVSyncEnabled;
            QualitySettings.vSyncCount = isVSyncEnabled ? 1 : 0;
        }
        else
        {
            vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        }

        if (PlayerPrefs.HasKey("TextureQuality"))
        {
            int textureQuality = PlayerPrefs.GetInt("TextureQuality");
            textureQualityDropdown.value = textureQuality;
            QualitySettings.globalTextureMipmapLimit = textureQuality;
        }

        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropdown.value = resolutionIndex;

            SetResolution(resolutionIndex);
        }

        if (PlayerPrefs.HasKey("FullscreenMode"))
        {
            int fullscreenModeIndex = PlayerPrefs.GetInt("FullscreenMode");
            fullscreenModeDropdown.value = fullscreenModeIndex;
            SetFullscreenMode(fullscreenModeIndex);
        }
    }

    private void SetResolution(int index)
    {
        Resolution[] resolutions = Screen.resolutions;
        if (index >= 0 && index < resolutions.Length)
        {
            Resolution selectedResolution = resolutions[index];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        }
    }

    private void SetFullscreenMode(int index)
    {
        switch (index)
        {
            case 0: // Fenstermodus
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1: // Vollbildmodus
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2: // Fenster im Vollbildmodus
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }

        Screen.fullScreen = index != 0;  
    }
}
