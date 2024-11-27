using UnityEngine;

public class CharacterCustomizationButtons : MonoBehaviour
{
    [SerializeField] private int CharacterSettingsIndex;
    [SerializeField] private CharacterCustomization characterCustomization;
    [SerializeField] bool isIncrement;
    public void ButtonPressed() {
        characterCustomization.ChangeIndex(characterCustomization.Settings.CharacterSettings[CharacterSettingsIndex], isIncrement);
    }
}
