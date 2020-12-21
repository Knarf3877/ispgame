using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject playerHUD;
    public GameObject playerStats;
    bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        isPaused = false;
        Debug.Log("this is working");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && Respawn.isDead == false)
        {
            PauseGame();
            Debug.Log("game paused");
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused && Respawn.isDead == false)
        {
            ResumeGame();
        }
    }
    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        playerHUD.SetActive(false);
        playerStats.SetActive(false);
        isPaused = true;
    }

    public void ResumeGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        playerHUD.SetActive(true);
        playerStats.SetActive(true);
        isPaused = false;
    }
    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isPaused = false;
    }
}
