using UnityEngine;
using TMPro;
using System;  

public class TextureQualityManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown textureQualityDropdown;

    private int currentTextureQuality = 1;

    void Start()
    {
        PopulateDropdown();

        textureQualityDropdown.onValueChanged.AddListener(OnTextureQualityChanged);

        ApplyTextureQuality(currentTextureQuality);
    }

    private void PopulateDropdown()
    {
        textureQualityDropdown.ClearOptions();
        
        var options = new System.Collections.Generic.List<string>
        {
            "Low",        // Option 0 - Niedrig
            "Medium",     // Option 1 - Mittel
            "High"        // Option 2 - Hoch
        };
        
        textureQualityDropdown.AddOptions(options);

        textureQualityDropdown.value = currentTextureQuality;
    }

    private void OnTextureQualityChanged(int index)
    {
        currentTextureQuality = index;
        ApplyTextureQuality(currentTextureQuality);
    }

    private void ApplyTextureQuality(int textureQuality)
    {
        switch (textureQuality)
        {
            case 0:
                QualitySettings.globalTextureMipmapLimit = 2; 
                break;

            case 1:
                QualitySettings.globalTextureMipmapLimit = 1;  
                break;

            case 2:
                QualitySettings.globalTextureMipmapLimit = 0;  
                break;
        }
    }
}
