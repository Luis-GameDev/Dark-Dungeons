using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharacterSettings
{
    public List<GameObject> PrefabObject;
    public int Index = -1; // -1 means: No prefab is active
}

[Serializable]
public class CharacterCustomizationSettings
{
    public List<CharacterSettings> CharacterSettings;
}

public class CharacterCustomization : MonoBehaviour
{
    [SerializeField] GameObject gameObjectToHide;
    public CharacterCustomizationSettings Settings;

    private string SaveFilePath;

    // <summary>
    // Changes the index of the specified CharacterSettings and activates the corresponding prefab.
    // </summary>
    // <param name="setting">The settings for which the index needs to be changed.</param>
    // <param name="isIncrement">If true, increment the index; otherwise, decrement it.</param>
    public void ChangeIndex(CharacterSettings setting, bool isIncrement)
    {
        if (setting == null || setting.PrefabObject == null)
        {
            Debug.LogWarning("Invalid CharacterSettings provided.");
            return;
        }

        // Find the corresponding CharacterSettings instance
        var objectsToChange = Settings.CharacterSettings.Find(v => v.PrefabObject == setting.PrefabObject);

        if (objectsToChange == null)
        {
            Debug.LogWarning("CharacterSettings not found in settings.");
            return;
        }

        int prefabCount = objectsToChange.PrefabObject.Count;
        int currentIndex = objectsToChange.Index;

        // Update the index with wrap-around logic
        if (isIncrement)
        {
            objectsToChange.Index = (currentIndex + 1) % prefabCount;
        }
        else
        {
            objectsToChange.Index = (currentIndex - 1 + prefabCount) % prefabCount;
        }

        // Activate or deactivate prefabs based on the new index
        for (int i = 0; i < prefabCount; i++)
        {
            if (objectsToChange.PrefabObject[i] != null)
            {
                objectsToChange.PrefabObject[i].SetActive(i == objectsToChange.Index);
            }
        }

        // Deactivate all prefabs if the index is -1
        if (objectsToChange.Index == -1)
        {
            foreach (var prefab in objectsToChange.PrefabObject)
            {
                if (prefab != null)
                    prefab.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Saves the current character configuration to a JSON file.
    /// </summary>
    public void SaveCharacter()
    {
        try
        {
            string json = JsonUtility.ToJson(Settings, true);
            File.WriteAllText(SaveFilePath, json);
            Debug.Log("Character settings saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save character settings: {ex.Message}");
        }

        // Hide the customization UI
        if (gameObjectToHide != null)
            gameObjectToHide.SetActive(false);
    }

    /// <summary>
    /// Called when the script instance is being loaded. 
    /// Loads the saved character settings and applies them to the character.
    /// </summary>
    private void Awake()
    {
        SaveFilePath = Path.Combine(Application.persistentDataPath, "CharacterSettings.json");
        
        if (File.Exists(SaveFilePath))
        {
            try
            {
                string json = File.ReadAllText(SaveFilePath);
                Settings = JsonUtility.FromJson<CharacterCustomizationSettings>(json);

                // Apply the saved settings to the character
                foreach (var setting in Settings.CharacterSettings)
                {
                    if (setting.Index == -1)
                    {
                        foreach (var prefab in setting.PrefabObject)
                        {
                            if (prefab != null)
                                prefab.SetActive(false);
                        }
                    }
                    else if (setting.Index >= 0 && setting.Index < setting.PrefabObject.Count)
                    {
                        for (int i = 0; i < setting.PrefabObject.Count; i++)
                        {
                            if (setting.PrefabObject[i] != null)
                                setting.PrefabObject[i].SetActive(i == setting.Index);
                        }
                    }
                }

                Debug.Log("Character settings loaded and applied.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load character settings: {ex.Message}");
            }
        }
        else
        {
            Debug.Log("No saved character settings found. Using default configuration.");
        }
    }
}
