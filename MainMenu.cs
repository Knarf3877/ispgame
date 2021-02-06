using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject options;
    public GameObject levels;
    public GameObject[] selectLevel;

    private void Start()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        levels.SetActive(false);
    }
    public void PlayGame()
    {
        mainMenu.SetActive(false);
        levels.SetActive(true);
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("game quit");
    }

    public void BackButton()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }

    public void GoToLevel()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        int currentValue = int.Parse(levelName.Substring(0, 1));
        Debug.Log(levelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + currentValue);
    }
}
