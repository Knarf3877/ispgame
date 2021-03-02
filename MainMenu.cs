using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject info;
    public GameObject levels;
    public GameObject settings;
    //public GameObject[] selectLevel;

    private void Start()
    {
        mainMenu.SetActive(true);
        info.SetActive(false);
        levels.SetActive(false);
        settings.SetActive(false);
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            BackButton();
        }

    }
    public void PlayGame()
    {
        mainMenu.SetActive(false);
        levels.SetActive(true);
    }

    public void Info()
    {
        mainMenu.SetActive(false);
        info.SetActive(true);
    }
    public void Settings()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("game quit");
    }

    public void BackButton()
    {
        if (!mainMenu.activeSelf)
        {
            mainMenu.SetActive(!mainMenu.activeSelf);
            this.gameObject.SetActive(!this.gameObject.activeSelf);
        }
    }

    public void GoToLevel()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        int currentValue = int.Parse(levelName.Substring(0, 1));
        Debug.Log(levelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + currentValue);
    }
}
