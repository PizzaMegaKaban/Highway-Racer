﻿//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Options manager that handles quality, gameplay, and controller settings.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/UI/HR UI Options Manager")]
public class HR_UIOptionsManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_UIOptionsManager instance;
    public static HR_UIOptionsManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_UIOptionsManager>();
            }

            return instance;
        }
    }
    #endregion

    public Toggle touch;
    public Toggle tilt;
    public Toggle joystick;
    [Space()]
    public Toggle low;
    public Toggle med;
    public Toggle high;
    [Space()]
    public Slider drawDistance;
    public Slider masterVolume;
    public Slider musicVolume;

    public delegate void OptionsChanged();
    public static event OptionsChanged OnOptionsChanged;

    private void OnEnable() {

        if (touch && tilt && joystick) {

            if (RCC_Settings.Instance.mobileControllerEnabled) {

                if (PlayerPrefs.GetInt("ControllerType", 0) == 0) {

                    touch.isOn = true;
                    tilt.isOn = false;
                    joystick.isOn = false;

                }

                if (PlayerPrefs.GetInt("ControllerType", 0) == 1) {

                    touch.isOn = false;
                    tilt.isOn = true;
                    joystick.isOn = false;

                }

                if (PlayerPrefs.GetInt("ControllerType", 0) == 3) {

                    touch.isOn = false;
                    tilt.isOn = false;
                    joystick.isOn = true;

                }

            }

        }

        if (QualitySettings.GetQualityLevel() == 0) {

            low.isOn = true;
            high.isOn = false;
            med.isOn = false;

        }

        if (QualitySettings.GetQualityLevel() == 1) {

            low.isOn = false;
            high.isOn = false;
            med.isOn = true;

        }

        if (QualitySettings.GetQualityLevel() == 2) {

            low.isOn = false;
            high.isOn = true;
            med.isOn = false;

        }

        drawDistance.value = PlayerPrefs.GetFloat("DrawDistance", 300);
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume", .35f);

    }

    public void OnUpdate() {

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void SetControllerType(Toggle toggle) {

        if (toggle.isOn) {

            toggle.isOn = false;
            return;

        }

        switch (toggle.name) {

            case "Touchscreen":
                PlayerPrefs.SetInt("ControllerType", 0);
                RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);
                touch.isOn = true;
                tilt.isOn = false;
                joystick.isOn = false;
                break;
            case "Accelerometer":
                PlayerPrefs.SetInt("ControllerType", 1);
                RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
                touch.isOn = false;
                tilt.isOn = true;
                joystick.isOn = false;
                break;
            case "SteeringWheel":
                PlayerPrefs.SetInt("ControllerType", 2);
                RCC.SetMobileController(RCC_Settings.MobileController.SteeringWheel);
                break;
            case "Joystick":
                PlayerPrefs.SetInt("ControllerType", 3);
                RCC.SetMobileController(RCC_Settings.MobileController.Joystick);
                touch.isOn = false;
                tilt.isOn = false;
                joystick.isOn = true;
                break;

        }

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void SetMasterVolume(Slider slider) {

        PlayerPrefs.SetFloat("MasterVolume", slider.value);

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void SetMusicVolume(Slider slider) {

        PlayerPrefs.SetFloat("MusicVolume", slider.value);

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void SetQuality(Toggle toggle) {

        if (toggle.isOn) {

            toggle.isOn = false;
            return;

        }

        switch (toggle.name) {

            case "Low":
                QualitySettings.SetQualityLevel(0);
                high.isOn = false;
                med.isOn = false;
                break;
            case "Medium":
                QualitySettings.SetQualityLevel(1);
                low.isOn = false;
                high.isOn = false;
                break;
            case "High":
                QualitySettings.SetQualityLevel(2);
                low.isOn = false;
                med.isOn = false;
                break;

        }

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void SetDrawDistance(Slider slider) {

        PlayerPrefs.SetInt("DrawDistance", (int)slider.value);

        if (OnOptionsChanged != null)
            OnOptionsChanged();

    }

    public void QuitGame() {

        Application.Quit();

    }

}
