using UnityEngine;
using TMPro; 
using System.Collections.Generic;

public class ResolutionDropdown : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown; 

    private List<(Vector2 resolution, string name)> resolutions = new List<(Vector2, string)>()
    {
        (new Vector2(1920, 1080), "Full HD"),     
        (new Vector2(3440, 1440), "Ultra Wide"),  
        (new Vector2(1280, 720), "HD"),           
        (new Vector2(2560, 1440), "QHD"),         
        (new Vector2(3840, 2160), "4K UHD")       
    };

    void Start()
    {
        PopulateDropdown();

        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void PopulateDropdown()
    {
        resolutionDropdown.ClearOptions(); 

        List<string> options = new List<string>(); 

        foreach (var res in resolutions)
        {
            options.Add(res.resolution.x + " x " + res.resolution.y + " (" + res.name + ")");
        }

        resolutionDropdown.AddOptions(options);
    }

    private void OnResolutionChanged(int index)
    {
        Vector2 selectedResolution = resolutions[index].resolution;
        Screen.SetResolution((int)selectedResolution.x, (int)selectedResolution.y, Screen.fullScreen);
    }
}
