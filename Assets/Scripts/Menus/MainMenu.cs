using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;
    public GameObject MiniGameMenu;
    
    void Awake()
    {
        GameProgress.checkPointLevel = PlayerPrefs.GetInt("CheckPoint", 0);

        if (GameProgress.checkPointLevel == 0) {
            continueButton.interactable = false;
        }
    }

    public void OnContinueClick()
    {
        // TODO:
        // * Load Inventory
        Inventory.ClearKeys();
        Invoke(nameof(LoadGameFromCheckPoint), .5f);
    }

    public void OnStartClick()
    {
        GameProgress.level = 0;
        Inventory.ClearInventory();
        Invoke(nameof(StartGame), .5f);
    }

    public void OnMiniGameClick()
    {
        MiniGameMenu.SetActive(true);
    }

    public void OnArtifactsClick()
    {
        Invoke(nameof(LoadArtifacts), .5f);
    }

    public void OnQuitClick()
    {
        Invoke(nameof(Quit), .5f);
    }

    void LoadGameFromCheckPoint()
    {
        GameProgress.GotoCheckPointLevel();
    }
    void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    void LoadArtifacts()
    {
        SceneManager.LoadScene("Artifacts");
    }

    void Quit()
    {
        Application.Quit();
    }
}
