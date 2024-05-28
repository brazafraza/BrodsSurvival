using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;

public class GameMenu : MonoBehaviour
{

    public bool opened;
    public enum MenuMode { Main, Pause}
    public MenuMode menuMode;

    public GameObject menuAlt;
    public bool settingsMenuActive = false;

    [Header("General")]
    public Transform UI;
    public GameObject newGameButton;
    public GameObject loadGameButton;
    [Space]
    [Space]
    public GameObject saveGameButton;
    public GameObject backToMainMenuButton;

    public GameObject settingsMenu;

    [Header("Main Menu")]
    public GameObject mainBackground;


    [Header("Pause Menu")]
    public GameObject pauseBackground;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (settingsMenuActive)
        {
            menuAlt.SetActive(false);
        }
        if (!settingsMenuActive)
        {
            menuAlt.SetActive(true);
        }

        if (menuMode == MenuMode.Main)
            
        {
            UI.transform.localPosition = new Vector3(0, 0, 0);
            Time.timeScale = 1;
        }
        else if(menuMode == MenuMode.Pause)
        {


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                opened = !opened;
            }
                

            if (opened)
            {
                UI.transform.localPosition = new Vector3(0, 0, 0); Time.timeScale = 0;
                Time.timeScale = 0;

             //   Cursor.visible = true;
               // Cursor.lockState = CursorLockMode.None;
            }    
            else
            {
                UI.transform.localPosition = new Vector3(-10000, 0, 0);
                Time.timeScale = 1;
                //Cursor.visible = false;
               // Cursor.lockState = CursorLockMode.Locked;
            }
                
        }
    }

    public void NewGame()
    {
        menuMode = MenuMode.Pause;

        if (mainBackground != null)
        {
            mainBackground.gameObject.SetActive(false);
        }
        if (pauseBackground != null)
        {
            pauseBackground.gameObject.SetActive(true);
        }

        newGameButton.gameObject.SetActive(false);
        loadGameButton.gameObject.SetActive(false);

        saveGameButton.gameObject.SetActive(true);
        backToMainMenuButton.gameObject.SetActive(true);

        SaveHandler.load = false;

        SceneManager.LoadScene(1);
    }

    public void LoadGame()
    {
        // CHECK THAT DIRECTORY & FILE HAVE BEEN CREATED
        if (!Directory.Exists(Application.dataPath + "/saves"))
        {
            return;
        }

        if (!File.Exists(Application.dataPath + "/saves/GameSave.save"))
        {
            return;
        }

        SaveHandler.load = true;

        menuMode = MenuMode.Pause;

        if (mainBackground != null)
        {
            mainBackground.gameObject.SetActive(false);
        }
        if (pauseBackground != null)
        {
            pauseBackground.gameObject.SetActive(true);
        }

        newGameButton.gameObject.SetActive(false);
        loadGameButton.gameObject.SetActive(false);

        saveGameButton.gameObject.SetActive(true);
        backToMainMenuButton.gameObject.SetActive(true);

        SceneManager.LoadScene(1);
    }
    public void BackToMain()
    {
        menuMode = MenuMode.Main;
        SceneManager.LoadScene(0);

        Destroy(gameObject);
    }

    public void SaveGame()
    {
        FindObjectOfType<SaveHandler>().Save();

        //delete save here
    }

    public void Settings()
    {
        settingsMenuActive = true;
        settingsMenu.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

}
