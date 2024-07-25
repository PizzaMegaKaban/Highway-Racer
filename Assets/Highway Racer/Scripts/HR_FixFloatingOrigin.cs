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
/// Fixes the floating origin when player gets too far away from the origin. Repositions all important and necessary gameobjects to the 0 point.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Gameplay/HR Fix Floating Origin")]
public class HR_FixFloatingOrigin : MonoBehaviour {

    private List<GameObject> targetGameObjects = new List<GameObject>();        //  Necessary gameobjects.
    public float zLimit = 1000f;        //  Target Z limit.

    private void ResetBack() {

        if (targetGameObjects == null)
            targetGameObjects = new List<GameObject>();

        targetGameObjects.Clear();

        if(HR_TrafficPooling.Instance)
            targetGameObjects.Add(HR_TrafficPooling.Instance.container);

        if(HR_RoadPooling.Instance)
            targetGameObjects.Add(HR_RoadPooling.Instance.allRoads);

        if(RCC_SceneManager.Instance.activePlayerVehicle)
            targetGameObjects.Add(RCC_SceneManager.Instance.activePlayerVehicle.gameObject);

        if(FindObjectOfType<HR_CarCamera>())
            targetGameObjects.Add(FindObjectOfType<HR_CarCamera>().gameObject);

        //  Creating parent gameobject. Adding necessary gameobjects, repositioning them, and lastly destroy the parent.
        GameObject parentGameObject = new GameObject("Parent");

        for (int i = 0; i < targetGameObjects.Count; i++)
            targetGameObjects[i].transform.SetParent(parentGameObject.transform, true);

        parentGameObject.transform.position -= Vector3.forward * zLimit;

        for (int i = 0; i < targetGameObjects.Count; i++)
            targetGameObjects[i].transform.SetParent(null);

        Destroy(parentGameObject);

    }

    private void Update() {

        //  If no player vehicle found, return.
        if (!RCC_SceneManager.Instance.activePlayerVehicle)
            return;

        //  Getting distance.
        float distance = RCC_SceneManager.Instance.activePlayerVehicle.transform.position.z;

        //  If distance exceeds the limits, reset.
        if (distance >= zLimit)
            ResetBack();

    }

}
