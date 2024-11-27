using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Settings
{
    [Serializable]
    public class GraphicsSettings
    {
        public int TextureQuality;
        public int ResolutionIndex;
        public int FullscreenModeIndex;
        public bool VSync;
    }

    public class GraphicsManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Dropdown textureQualityDropdown;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown fullscreenModeDropdown;
        [SerializeField] private Toggle vSyncToggle;

        private static GraphicsSettings graphicsSettings = new GraphicsSettings();
        private string settingsFilePath;

        void Awake()
        {
            settingsFilePath = Path.Combine(Application.persistentDataPath, "GraphicsSettings.json");
            LoadSettings();
            AddEventListeners();
        }

        public void OnSettingsClose()
        {
            SaveSettings();
        }

        private void SaveSettings()
        {
            graphicsSettings.TextureQuality = textureQualityDropdown.value;
            graphicsSettings.ResolutionIndex = resolutionDropdown.value;
            graphicsSettings.FullscreenModeIndex = fullscreenModeDropdown.value;
            graphicsSettings.VSync = vSyncToggle.isOn;

            string json = JsonUtility.ToJson(graphicsSettings, true);
            File.WriteAllText(settingsFilePath, json);
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                graphicsSettings = JsonUtility.FromJson<GraphicsSettings>(json);

                textureQualityDropdown.value = graphicsSettings.TextureQuality;
                resolutionDropdown.value = graphicsSettings.ResolutionIndex;
                fullscreenModeDropdown.value = graphicsSettings.FullscreenModeIndex;
                vSyncToggle.isOn = graphicsSettings.VSync;

                ApplySettings();
            }
            else
            {
                InitializeDefaultSettings();
            }
        }

        private void ApplySettings()
        {
            QualitySettings.vSyncCount = graphicsSettings.VSync ? 1 : 0;

            QualitySettings.globalTextureMipmapLimit = graphicsSettings.TextureQuality;

            SetResolution(graphicsSettings.ResolutionIndex);

            SetFullscreenMode(graphicsSettings.FullscreenModeIndex);
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
                case 0: // Windowed mode
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 1: // Fullscreen mode
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                case 2: // Maximized window
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                    break;
            }

            Screen.fullScreen = index != 0;
        }

        private void InitializeDefaultSettings()
        {
            graphicsSettings.TextureQuality = 0;
            graphicsSettings.ResolutionIndex = 0;
            graphicsSettings.FullscreenModeIndex = 1;
            graphicsSettings.VSync = QualitySettings.vSyncCount > 0;

            SaveSettings();
        }

        private void AddEventListeners()
        {
            textureQualityDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });
            resolutionDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });
            fullscreenModeDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });
            vSyncToggle.onValueChanged.AddListener(delegate { SaveSettings(); });
        }
    }
}
