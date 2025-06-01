using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Management;
using UnityEngine.SceneManagement;


public class GameStartMenu : MonoBehaviour
{
    [Header("UI Pages")]
    public GameObject mainMenu;
    public GameObject options;
    public GameObject about;

    [Header("Main Menu Buttons")]
    public Button startButton;
    public Button optionButton;
    public Button aboutButton;
    public Button quitButton;

    public List<Button> returnButtons;
    public FadeScreen fadeScreen; //

    // Start is called before the first frame update
    void Start()
    {
        /* XRInitializer에서 이미 초기화했다면 중복 방지
        var xrManager = XRGeneralSettings.Instance.Manager;
        if (xrManager != null && !xrManager.isInitializationComplete)
        {
            Debug.Log("GameStartMenu에서 XR 초기화");
            xrManager.InitializeLoaderSync();
            xrManager.StartSubsystems();

            var xrInput = xrManager.activeLoader?.GetLoadedSubsystem<XRInputSubsystem>();
            if (xrInput != null)
            {
                xrInput.TryRecenter();
                Debug.Log("XR Recentered in GameStartMenu");
            }
        }

        Debug.Log("GameStartMenu： XR Device Active: " + XRSettings.isDeviceActive);
        Debug.Log("GameStartMenu： XR Device Name: " + XRSettings.loadedDeviceName);
        


        // SceneTransitionManager.singleton.GoToScene(0); //
        
        //fadeScreen.FadeOut();
        // SceneManager.LoadScene(0);
        */

        EnableMainMenu();

        //Hook events
        startButton.onClick.AddListener(StartGame);
        optionButton.onClick.AddListener(EnableOption);
        aboutButton.onClick.AddListener(EnableAbout);
        quitButton.onClick.AddListener(QuitGame);

        foreach (var item in returnButtons)
        {
            item.onClick.AddListener(EnableMainMenu);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        HideAll();
        SceneTransitionManager.singleton.GoToSceneAsync(1);
    }

    public void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(false);
    }

    public void EnableMainMenu()
    {
        mainMenu.SetActive(true);
        options.SetActive(false);
        about.SetActive(false);
    }
    public void EnableOption()
    {
        mainMenu.SetActive(false);
        options.SetActive(true);
        about.SetActive(false);
    }
    public void EnableAbout()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        about.SetActive(true);
    }
}
