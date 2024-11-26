using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class optionsCategorySelect : MonoBehaviour
{
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    [SerializeField] private GameObject Graphics;
    [SerializeField] private GameObject GraphicsButton;

    [SerializeField] private GameObject Audio;
    [SerializeField] private GameObject AudioButton;

    [SerializeField] private GameObject Controls;
    [SerializeField] private GameObject ControlsButton;

    private void Start()
    {
        Graphics.SetActive(true);
        SetButtonColor(GraphicsButton, activeColor);

        Audio.SetActive(false);
        SetButtonColor(AudioButton, inactiveColor);

        Controls.SetActive(false);
        SetButtonColor(ControlsButton, inactiveColor);
    }

    public void openGraphics()
    {
        Graphics.SetActive(true);
        SetButtonColor(GraphicsButton, activeColor);

        Audio.SetActive(false);
        SetButtonColor(AudioButton, inactiveColor);

        Controls.SetActive(false);
        SetButtonColor(ControlsButton, inactiveColor);
    }

    public void openAudio()
    {
        Graphics.SetActive(false);
        SetButtonColor(GraphicsButton, inactiveColor);

        Audio.SetActive(true);
        SetButtonColor(AudioButton, activeColor);

        Controls.SetActive(false);
        SetButtonColor(ControlsButton, inactiveColor);
    }

    public void openControls()
    {
        Graphics.SetActive(false);
        SetButtonColor(GraphicsButton, inactiveColor);

        Audio.SetActive(false);
        SetButtonColor(AudioButton, inactiveColor);

        Controls.SetActive(true);
        SetButtonColor(ControlsButton, activeColor);
    }

    private void SetButtonColor(GameObject buttonObject, Color targetColor)
    {
        Button button = buttonObject.GetComponent<Button>();

        ColorBlock colors = button.colors;

        colors.normalColor = targetColor;

        button.colors = colors;
    }
}
