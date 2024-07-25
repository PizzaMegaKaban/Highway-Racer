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
/// Upgrades engine of the car controller.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Upgrade/HR Engine")]
public class HR_VehicleUpgrade_Engine : MonoBehaviour {

    private RCC_CarControllerV3 carController;
    public RCC_CarControllerV3 CarController {

        get {

            if (carController == null)
                carController = GetComponentInParent<RCC_CarControllerV3>();

            return carController;

        }

    }

    private int _engineLevel = 0;
    public int engineLevel {
        get {
            return _engineLevel;
        }
        set {
            if (value <= 5)
                _engineLevel = value;
        }
    }

    private float defEngine = -1;
    private float maxEngine = 750f;

    public void Initialize() {

        if (defEngine == -1)
            defEngine = CarController.maxEngineTorque;

        maxEngine = CarController.GetComponent<HR_PlayerHandler>().maxEngineTorque;

        //  Setting upgraded engine torque if saved.
        engineLevel = PlayerPrefs.GetInt(transform.root.name + "EngineLevel");
        CarController.maxEngineTorque = Mathf.Lerp(defEngine, maxEngine, engineLevel / 5f);

    }

    /// <summary>
    /// Updates engine torque and save it.
    /// </summary>
    public void UpdateStats() {

        CarController.maxEngineTorque = Mathf.Lerp(defEngine, maxEngine, engineLevel / 5f);
        PlayerPrefs.SetInt(transform.root.name + "EngineLevel", engineLevel);

    }

    private void Update() {

        if (!carController)
            return;

        //  Make sure max torque is not smaller.
        if (maxEngine < CarController.maxEngineTorque)
            maxEngine = CarController.maxEngineTorque;

    }

}
