using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Settings
{
    [Serializable]
    public class GameSettings
    {
        public List<VolumeSetting> Volumes = new List<VolumeSetting>();
    }

    [Serializable]
    public class VolumeSetting
    {
        public string name;
        public float value;
    }

    public class AudioManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject SliderPrefab;

        [Header("Audio References")]
        [SerializeField] private AudioMixer audioMixer;

        private static GameSettings gameSettings = new GameSettings();
        private string settingsFilePath;
        private AudioMixerGroup[] audioGroups;

        void Awake()
        {
            settingsFilePath = Path.Combine(Application.persistentDataPath, "AudioSettings.json");
            audioGroups = audioMixer.FindMatchingGroups(string.Empty);

            // Initialize sliders for each audio group
            foreach (var group in audioGroups)
            {
                CreateSliderForGroup(group);
            }

            // Load saved settings asynchronously
            StartCoroutine(LoadSettingsCoroutine());
        }

        /// <summary>
        /// Creates a slider UI element for a given audio group and sets up event listeners.
        /// </summary>
        private void CreateSliderForGroup(AudioMixerGroup group)
        {
            GameObject slider = Instantiate(SliderPrefab, transform);
            AudioSliderManager asm = slider.GetComponent<AudioSliderManager>();
            asm.nameText.text = group.name;

            Slider sliderScript = slider.GetComponentInChildren<Slider>();
            sliderScript.onValueChanged.AddListener(value =>
            {
                UpdateSliderDisplay(asm, value);
                SetVolume(group.name + "Volume", value);
            });
        }

        /// <summary>
        /// Updates the slider display text based on the current value.
        /// </summary>
        private void UpdateSliderDisplay(AudioSliderManager asm, float value)
        {
            if (value > 0)
            {
                asm.valueText.text = $"{math.round(math.remap(0f, 1f, 100f, 200f, value))}%";
            }
            else
            {
                asm.valueText.text = $"{math.round(math.remap(-4f, 0f, 0f, 100f, value))}%";
            }
        }

        /// <summary>
        /// Applies saved settings to the audio mixer and updates UI sliders.
        /// </summary>
        private void ApplySettings()
        {
            foreach (var group in audioGroups)
            {
                string parameterName = group.name + "Volume";
                VolumeSetting setting = gameSettings.Volumes.Find(v => v.name == parameterName);

                if (setting != null)
                {
                    // Update slider value and apply the volume
                    foreach (var asm in GetComponentsInChildren<AudioSliderManager>())
                    {
                        if (asm.nameText.text == group.name)
                        {
                            asm.slider.value = setting.value;
                            break;
                        }
                    }

                    SetVolume(parameterName, setting.value);
                }
            }
        }

        /// <summary>
        /// Sets the volume for a given parameter and saves the setting.
        /// </summary>
        public void SetVolume(string parameterName, float targetValue)
        {
            // Find or create a volume setting
            VolumeSetting setting = gameSettings.Volumes.Find(v => v.name == parameterName);
            if (setting != null)
            {
                setting.value = targetValue;
            }
            else
            {
                gameSettings.Volumes.Add(new VolumeSetting { name = parameterName, value = targetValue });
            }

            // Remap the value and apply it to the audio mixer
            float remappedValue = math.remap(-4f, 1f, -80f, 20f, targetValue);
            audioMixer.SetFloat(parameterName, remappedValue);

            SaveSettings();
        }

        /// <summary>
        /// Saves the current settings to a JSON file.
        /// </summary>
        private void SaveSettings()
        {
            string json = JsonUtility.ToJson(gameSettings, true);
            File.WriteAllText(settingsFilePath, json);
        }

        /// <summary>
        /// Loads settings from a JSON file asynchronously.
        /// </summary>
        private IEnumerator LoadSettingsCoroutine()
        {
            if (File.Exists(settingsFilePath))
            {
                string json = File.ReadAllText(settingsFilePath);
                gameSettings = JsonUtility.FromJson<GameSettings>(json);

                yield return null;

                ApplySettings();
            }
            else
            {
                // Create new settings if none exist
                InitializeDefaultSettings();
                yield return new WaitForSeconds(1f);
            }
        }

        /// <summary>
        /// Initializes default settings for all audio groups.
        /// </summary>
        private void InitializeDefaultSettings()
        {
            foreach (var group in audioGroups)
            {
                SetVolume(group.name + "Volume", 0f);
            }
        }
    }
}
