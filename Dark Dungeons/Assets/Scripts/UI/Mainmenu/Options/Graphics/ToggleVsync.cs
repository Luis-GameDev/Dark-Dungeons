using UnityEngine;
using UnityEngine.UI; // Für den Zugriff auf den Toggle

public class VSyncToggle : MonoBehaviour
{
    // Referenz zum Toggle, das V-Sync steuert
    [SerializeField] private Toggle vSyncToggle;

    void Start()
    {
        // Initialisiere den Toggle-Status basierend auf der aktuellen V-Sync-Einstellung
        vSyncToggle.isOn = QualitySettings.vSyncCount > 0;

        // Füge einen Listener hinzu, um auf Änderungen des Toggles zu reagieren
        vSyncToggle.onValueChanged.AddListener(OnVSyncToggleChanged);
    }

    // Diese Methode wird aufgerufen, wenn der Toggle-Status geändert wird
    private void OnVSyncToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // Aktiviere V-Sync, wenn der Toggle eingeschaltet ist
            QualitySettings.vSyncCount = 1;  // V-Sync einschalten (synchronisiert mit der Monitor-Refresh-Rate)
        }
        else
        {
            // Deaktiviere V-Sync, wenn der Toggle ausgeschaltet ist
            QualitySettings.vSyncCount = 0;  // V-Sync ausschalten (maximale Bildrate ohne Begrenzung)
        }
    }
}
