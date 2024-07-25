//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Upgrades max speed of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Speed")]
public class HR_VehicleUpgrade_Speed : MonoBehaviour {

    private RCC_CarControllerV3 carController;
    public RCC_CarControllerV3 CarController {

        get {

            if (carController == null)
                carController = GetComponentInParent<RCC_CarControllerV3>();

            return carController;

        }

    }

    private int _speedLevel = 0;
    public int speedLevel {
        get {
            return _speedLevel;
        }
        set {
            if (value <= 5)
                _speedLevel = value;
        }
    }

    private float defSpeed = -1;
    private float maxSpeed = 280f;

    public void Initialize() {

        if (defSpeed == -1)
            defSpeed = CarController.maxspeed;

        maxSpeed = CarController.GetComponent<HR_PlayerHandler>().maxSpeed;

        //  Setting upgraded speed if saved.
        speedLevel = PlayerPrefs.GetInt(transform.root.name + "SpeedLevel");
        CarController.maxspeed = Mathf.Lerp(defSpeed, maxSpeed, speedLevel / 5f);

    }

    /// <summary>
    /// Updates max speed and save it.
    /// </summary>
    public void UpdateStats() {

        if (!CarController)
            return;

        CarController.maxspeed = Mathf.Lerp(defSpeed, maxSpeed, speedLevel / 5f);
        PlayerPrefs.SetInt(transform.root.name + "SpeedLevel", speedLevel);

    }

    private void Update() {

        if (!CarController)
            return;

        //  Make sure max speed is not smaller.
        if (maxSpeed < CarController.maxspeed)
            maxSpeed = CarController.maxspeed;

    }

}
