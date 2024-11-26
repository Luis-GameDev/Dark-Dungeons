using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttons : MonoBehaviour
{
    [SerializeField] private GameObject optionsWindow;
    public void play() {
        SceneManager.LoadScene(1);
    }

    public void options() {
        optionsWindow.SetActive(true);
    }

    public void exit() {
        Application.Quit();
    }
}
