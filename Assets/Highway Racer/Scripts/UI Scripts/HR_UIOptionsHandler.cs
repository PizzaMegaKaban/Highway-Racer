//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Highway Racer/UI/HR UI Gameplay Options Handler")]
public class HR_UIOptionsHandler : MonoBehaviour {

    public GameObject pausedMenu;
    public GameObject pausedButtons;
    public GameObject optionsMenu;
    public GameObject restartButton;

    private void OnEnable() {

        HR_GamePlayHandler.OnPaused += OnPaused;
        HR_GamePlayHandler.OnResumed += OnResumed;

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
            restartButton.SetActive(false);
        else
            restartButton.SetActive(true);

    }

    public void ResumeGame() {

        HR_GamePlayHandler.Instance.Paused();

    }

    public void RestartGame() {

        HR_GamePlayHandler.Instance.RestartGame();

    }

    public void MainMenu() {

        HR_GamePlayHandler.Instance.MainMenu();

    }

    public void OptionsMenu(bool open) {

        optionsMenu.SetActive(open);

        if (open)
            pausedButtons.SetActive(false);
        else
            pausedButtons.SetActive(true);

    }

    private void OnPaused() {

        pausedMenu.SetActive(true);
        pausedButtons.SetActive(true);

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
            return;

        AudioListener.pause = true;
        Time.timeScale = 0;

    }

    public void OnResumed() {

        pausedMenu.SetActive(false);
        pausedButtons.SetActive(false);

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
            return;

        AudioListener.pause = false;
        Time.timeScale = 1;

    }

    public void ChangeCamera() {

        if (FindObjectOfType<HR_CarCamera>())
            FindObjectOfType<HR_CarCamera>().ChangeCamera();

    }

    private void OnDisable() {

        HR_GamePlayHandler.OnPaused -= OnPaused;
        HR_GamePlayHandler.OnResumed -= OnResumed;

    }

}
