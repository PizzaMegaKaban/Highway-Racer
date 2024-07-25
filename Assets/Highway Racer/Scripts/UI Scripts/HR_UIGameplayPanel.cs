//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[AddComponentMenu("BoneCracker Games/Highway Racer/UI/HR UI Gameplay Panel")]
public class HR_UIGameplayPanel : MonoBehaviour {

    private HR_PlayerHandler player;
    public GameObject content;

    public Text score;
    public Text timeLeft;
    public Text combo;

    public Text speed;
    public Text distance;
    public Text highSpeed;
    public Text oppositeDirection;

    public Slider damageSlider;
    public Slider bombSlider;

    private Image comboMImage;
    private Vector2 comboDefPos;

    private Image highSpeedImage;
    private Vector2 highSpeedDefPos;

    private Image oppositeDirectionImage;
    private Vector2 oppositeDirectionDefPos;

    private Image timeAttackImage;

    private RectTransform bombRect;
    private Vector2 bombDefPos;

    private GameObject multiplayerPanel;

    private void Awake() {

        if (combo) {

            comboMImage = combo.GetComponentInParent<Image>();
            comboDefPos = comboMImage.rectTransform.anchoredPosition;

        }

        if (highSpeed) {

            highSpeedImage = highSpeed.GetComponentInParent<Image>();
            highSpeedDefPos = highSpeedImage.rectTransform.anchoredPosition;

        }

        if (oppositeDirection) {

            oppositeDirectionImage = oppositeDirection.GetComponentInParent<Image>();
            oppositeDirectionDefPos = oppositeDirectionImage.rectTransform.anchoredPosition;

        }

        if (timeLeft)
            timeAttackImage = timeLeft.GetComponentInParent<Image>();

        if (bombSlider) {

            bombRect = bombSlider.GetComponent<RectTransform>();
            bombDefPos = bombRect.anchoredPosition;

        }

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON

        HR_UI_GameplayPhoton multiplayerPanelScript = GetComponentInChildren<HR_UI_GameplayPhoton>(true);

        if (multiplayerPanelScript)
            multiplayerPanel = multiplayerPanelScript.gameObject;

#endif

    }

    private void OnEnable() {

        HR_GamePlayHandler.OnPlayerSpawned += HR_PlayerHandler_OnPlayerSpawned;
        HR_GamePlayHandler.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;

    }

    private void Start() {

        if (multiplayerPanel) {

            if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
                multiplayerPanel.SetActive(true);
            else
                multiplayerPanel.SetActive(false);

        }

    }

    private void HR_PlayerHandler_OnPlayerSpawned(HR_PlayerHandler _player) {

        player = _player;
        content.SetActive(true);

    }

    private void HR_PlayerHandler_OnPlayerDied(HR_PlayerHandler _player, int[] scores) {

        player = null;
        content.SetActive(false);

    }

    private void Update() {

        if (!player)
            return;

        if (combo) {

            if (player.combo > 1)
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, comboDefPos, Time.deltaTime * 5f);
            else
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, new Vector2(comboDefPos.x - 500, comboDefPos.y), Time.deltaTime * 5f);

        }

        if (highSpeed) {

            if (player.highSpeedCurrent > .1f)
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, highSpeedDefPos, Time.deltaTime * 5f);
            else
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, new Vector2(highSpeedDefPos.x + 500, highSpeedDefPos.y), Time.deltaTime * 5f);

        }

        if (oppositeDirection) {

            if (player.opposideDirectionCurrent > .1f)
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, oppositeDirectionDefPos, Time.deltaTime * 5f);
            else
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, new Vector2(oppositeDirectionDefPos.x - 500, oppositeDirectionDefPos.y), Time.deltaTime * 5f);

        }

        if (timeLeft) {

            if (HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.TimeAttack) {

                if (!timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(true);

            } else {

                if (timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(false);

            }

        }

        if (damageSlider) {

            damageSlider.value = player.damage;

        }

        if (bombSlider) {

            if (HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.Bomb) {

                if (!bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(true);

            } else {

                if (bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(false);

            }

            if (player.bombTriggered)
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, bombDefPos, Time.deltaTime * 5f);
            else
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, new Vector2(bombDefPos.x - 500, bombDefPos.y), Time.deltaTime * 5f);

        }

    }

    private void LateUpdate() {

        if (!player)
            return;

        if (score)
            score.text = player.score.ToString("F0");

        if (speed)
            speed.text = player.speed.ToString("F0");

        if (distance)
            distance.text = (player.distance).ToString("F2");

        if (highSpeed)
            highSpeed.text = player.highSpeedCurrent.ToString("F1");

        if (oppositeDirection)
            oppositeDirection.text = player.opposideDirectionCurrent.ToString("F1");

        if (timeLeft)
            timeLeft.text = player.timeLeft.ToString("F1");

        if (combo)
            combo.text = player.combo.ToString();

        if (bombSlider && HR_GamePlayHandler.Instance.mode == HR_GamePlayHandler.Mode.Bomb)
            bombSlider.value = player.bombHealth / 100f;

    }

    private void OnDisable() {

        HR_GamePlayHandler.OnPlayerSpawned -= HR_PlayerHandler_OnPlayerSpawned;
        HR_GamePlayHandler.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;

    }

}
