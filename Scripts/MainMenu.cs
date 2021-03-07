using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject info;
    public GameObject levels;
    public GameObject settings;
    public AudioMixer audioMixer;
   // public Slider volume;
    public TMP_Dropdown resolutionDropdown;
    //public GameObject[] selectLevel;
    public Resolution[] resolutions;
    bool isFirstTime = true;

    private void Start()
    {
        
        mainMenu.SetActive(true);
        info.SetActive(false);
        levels.SetActive(false);
        int currentResIndex = 0;
       if (isFirstTime)
        {
            Screen.SetResolution(Screen.width, Screen.height, false);
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();

            for (int i = 0; i < resolutions.Length; i++)
            {
                string Option = resolutions[i].width + "x" + resolutions[i].height;
                options.Add(Option);

                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height)
                {
                    currentResIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResIndex;
            resolutionDropdown.RefreshShownValue();
            isFirstTime = false;
        }
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
           // gameObject.SetActive(!this.gameObject.activeSelf);
            settings.SetActive(false);
            info.SetActive(false);
            levels.SetActive(false);

        }
    }

    public void GoToLevel()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        int currentValue = int.Parse(levelName.Substring(0, 1));
        Debug.Log(levelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + currentValue);
    }

    public void SetScreenRes(int resIndex) {
      
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        Debug.Log(isFullscreen);
    }

    public void SetQuality(int quality) {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetVolume(float value) {
        Debug.Log(value);
        audioMixer.SetFloat("MasterVolume", value);
    }
}
