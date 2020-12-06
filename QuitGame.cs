using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{

    public void Start()
    {
        Debug.Log("Button Working");
    }
    public void ExitGame ()
    {
        Application.Quit();
        Debug.Log("game closed");
        Application.Quit();
    }

}

