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
using UnityEditor;

[CustomEditor(typeof(HR_ModApplier))]
public class HR_ModApplierEditor : Editor {

    HR_ModApplier prop;


    public override void OnInspectorGUI() {

        prop = (HR_ModApplier)target;
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();

        if (!prop.SpoilerManager) {

            EditorGUILayout.HelpBox("Spoiler Manager not found!", MessageType.Error);

            if (GUILayout.Button("Create")) {

                GameObject create = Instantiate(Resources.Load<GameObject>("Mod Setups/Spoilers"), prop.transform.position, prop.transform.rotation, prop.transform);
                create.transform.SetParent(prop.transform);
                create.transform.localPosition = Vector3.zero;
                create.transform.localRotation = Quaternion.identity;
                create.name = Resources.Load<GameObject>("Mod Setups/Spoilers").name;

            }

        } else {

            EditorGUILayout.HelpBox("Spoiler Manager found!", MessageType.None);

            if (GUILayout.Button("Select"))
                Selection.activeObject = prop.SpoilerManager.gameObject;

        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (!prop.SirenManager) {

            EditorGUILayout.HelpBox("Siren Manager not found!", MessageType.Error);

            if (GUILayout.Button("Create")) {

                GameObject create = Instantiate(Resources.Load<GameObject>("Mod Setups/Sirens"), prop.transform.position, prop.transform.rotation, prop.transform);
                create.transform.SetParent(prop.transform);
                create.transform.localPosition = Vector3.zero;
                create.transform.localRotation = Quaternion.identity;
                create.name = Resources.Load<GameObject>("Mod Setups/Sirens").name;

            }

        } else {

            EditorGUILayout.HelpBox("Siren Manager found!", MessageType.None);

            if (GUILayout.Button("Select"))
                Selection.activeObject = prop.SirenManager.gameObject;

        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (!prop.UpgradeManager) {

            EditorGUILayout.HelpBox("Upgrade Manager not found!", MessageType.Error);

            if (GUILayout.Button("Create")) {

                GameObject create = Instantiate(Resources.Load<GameObject>("Mod Setups/Upgrades"), prop.transform.position, prop.transform.rotation, prop.transform);
                create.transform.SetParent(prop.transform);
                create.transform.localPosition = Vector3.zero;
                create.transform.localRotation = Quaternion.identity;
                create.name = Resources.Load<GameObject>("Mod Setups/Upgrades").name;

            }

        } else {

            EditorGUILayout.HelpBox("Upgrade Manager found!", MessageType.None);

            if (GUILayout.Button("Select"))
                Selection.activeObject = prop.UpgradeManager.gameObject;

        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (!prop.PaintManager) {

            EditorGUILayout.HelpBox("Paint Manager not found!", MessageType.Error);

            if (GUILayout.Button("Create")) {

                GameObject create = Instantiate(Resources.Load<GameObject>("Mod Setups/Paints"), prop.transform.position, prop.transform.rotation, prop.transform);
                create.transform.SetParent(prop.transform);
                create.transform.localPosition = Vector3.zero;
                create.transform.localRotation = Quaternion.identity;
                create.name = Resources.Load<GameObject>("Mod Setups/Paints").name;

            }

        } else {

            EditorGUILayout.HelpBox("Paint Manager found!", MessageType.None);

            if (GUILayout.Button("Select"))
                Selection.activeObject = prop.PaintManager.gameObject;

        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        if (!prop.WheelManager) {

            EditorGUILayout.HelpBox("Wheel Manager not found!", MessageType.Error);

            if (GUILayout.Button("Create")) {

                GameObject create = Instantiate(Resources.Load<GameObject>("Mod Setups/Wheels"), prop.transform.position, prop.transform.rotation, prop.transform);
                create.transform.SetParent(prop.transform);
                create.transform.localPosition = Vector3.zero;
                create.transform.localRotation = Quaternion.identity;
                create.name = Resources.Load<GameObject>("Mod Setups/Wheels").name;

            }

        } else {

            EditorGUILayout.HelpBox("Wheel Manager found!", MessageType.None);

            if (GUILayout.Button("Select"))
                Selection.activeObject = prop.WheelManager.gameObject;

        }

        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();

    }

}
