using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    [SerializeField] private GameObject characterWindow;

    public void play() {
        SceneManager.LoadScene(1);
    }

    public void options() {
        optionsWindow.SetActive(true);
    }

    public void character() {
        characterWindow.SetActive(true);
    }

    public void exit() {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
