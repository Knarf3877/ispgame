using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    private void Start()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("game quit");
    }

    public void ToggleOptionsMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        options.SetActive(!options.activeSelf);
    }
}
