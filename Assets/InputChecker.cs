using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputChecker : MonoBehaviour
{
    [SerializeField] GameScreen gameScreen;
    [SerializeField] AudioController audioController;
    [SerializeField] PauseMenu pauseMenu;

    public static InputChecker Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (gameScreen)
            {
                case GameScreen.MainMenu:
                    Application.Quit();
                    break;
                case GameScreen.OptionMenu:
                    SceneManager.LoadScene("Menu_Pricipal");
                    if(audioController != null)
                    {
                        audioController.OnMasterChange(1f);
                        audioController.OnMusicToggle(true);
                        audioController.OnSfxToggle(true);
                    }
                    break;
                case GameScreen.Menu:
                    SceneManager.LoadScene("Menu_Pricipal");
                    break;
                case GameScreen.Level:
                    if(pauseMenu != null)
                    {
                        switch(pauseMenu.state)
                        {
                            case PauseMenu.PauseState.OnPause:
                                pauseMenu.OnExitConfirmWindow();
                                pauseMenu.OnPauseExit();
                                break;
                            case PauseMenu.PauseState.NoPause:
                                pauseMenu.OnPauseEnter();
                                break;
                        }
                    }
                    break;
            }
        }
    }

    public enum GameScreen
    {
        none,
        MainMenu,
        OptionMenu,
        Menu,
        Level
    }
}
