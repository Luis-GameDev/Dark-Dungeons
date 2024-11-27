using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    private List<(string key, string action)> actions = new List<(string key, string action)>() {
        ("W", "Walk Forward"),
        ("A", "Walk Left"),
        ("S", "Walk Backwards"),
        ("D", "Walk Right"),
        ("Space", "Jump"),
        ("E", "Interact"),
    };
    void Awake()
    {
        foreach (var action in actions) {
            GameObject button = Instantiate(buttonPrefab, transform);
            button.transform.Find("Key").GetComponent<TextMeshProUGUI>().text = action.key;
            button.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = action.action;
        }
    }
}
