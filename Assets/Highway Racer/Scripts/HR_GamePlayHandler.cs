//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2023 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
#if PHOTON_UNITY_NETWORKING
using Photon;
using Photon.Pun;
using Photon.Realtime;
#endif

/// <summary>
/// Gameplay management. Spawns player vehicle, sets volume, set mods, listens player events.
/// </summary>
[AddComponentMenu("BoneCracker Games/Highway Racer/Gameplay/HR Gameplay Handler")]
public class HR_GamePlayHandler : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_GamePlayHandler instance;
    public static HR_GamePlayHandler Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_GamePlayHandler>();
            }

            return instance;
        }
    }
    #endregion

    [Header("Time Of The Scene")]
    public DayOrNight dayOrNight = DayOrNight.Day;
    public enum DayOrNight { Day, Night }

    [Header("Current Mode")]
    public Mode mode = Mode.OneWay;
    public enum Mode { OneWay, TwoWay, TimeAttack, Bomb }

    [Header("Spawn Location Of The Cars")]
    public Transform spawnLocation;

    public HR_PlayerHandler player;
    public HR_PlayerHandler player2;

    public float overtakingDistance = 0f;
    public float maxDistance = 100f;

    private int selectedCarIndex = 0;
    private int selectedModeIndex = 0;

    public bool gameStarted = false;
    private bool paused = false;
    private readonly float minimumSpeed = 20f;

    public delegate void onCountDownStarted();
    public static event onCountDownStarted OnCountDownStarted;

    public delegate void onRaceStarted();
    public static event onRaceStarted OnRaceStarted;

    public delegate void onPlayerSpawned(HR_PlayerHandler player);
    public static event onPlayerSpawned OnPlayerSpawned;

    public delegate void onPlayerDied(HR_PlayerHandler player, int[] scores);
    public static event onPlayerDied OnPlayerDied;

    public delegate void onPaused();
    public static event onPaused OnPaused;

    public delegate void onResumed();
    public static event onResumed OnResumed;

    private AudioSource gameplaySoundtrack;

    private void Awake() {

        //  Make sure time scale is 1. We are setting volume to 0, we'll be increase it smoothly in the update method.
        Time.timeScale = 1f;
        AudioListener.volume = 0f;
        AudioListener.pause = false;
        gameStarted = false;

        //  Creating soundtrack.
        if (HR_HighwayRacerProperties.Instance.gameplayClips != null && HR_HighwayRacerProperties.Instance.gameplayClips.Length > 0) {

            gameplaySoundtrack = HR_CreateAudioSource.NewAudioSource(gameObject, "GamePlay Soundtrack", 0f, 0f, .35f, HR_HighwayRacerProperties.Instance.gameplayClips[UnityEngine.Random.Range(0, HR_HighwayRacerProperties.Instance.gameplayClips.Length)], true, true, false);
            gameplaySoundtrack.volume = PlayerPrefs.GetFloat("MusicVolume", .35f);
            gameplaySoundtrack.ignoreListenerPause = true;

        }

        //  Getting selected player car index and mode index.
        selectedCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex");
        selectedModeIndex = PlayerPrefs.GetInt("SelectedModeIndex");

        //  Setting proper mode.
        switch (selectedModeIndex) {

            case 0:
                mode = Mode.OneWay;
                break;
            case 1:
                mode = Mode.TwoWay;
                break;
            case 2:
                mode = Mode.TimeAttack;
                break;
            case 3:
                mode = Mode.Bomb;
                break;

        }

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1) {

            if (!PhotonNetwork.IsConnectedAndReady || !PhotonNetwork.InRoom) {

                Debug.LogError("Not connected to the server, or not in a room yet. Returning to the main menu...");
                SceneManager.LoadScene(0);
                return;

            }

            HR_FixFloatingOrigin fixFloatingOrigin = FindObjectOfType<HR_FixFloatingOrigin>();

            if (fixFloatingOrigin)
                Destroy(fixFloatingOrigin);

        }

#endif

    }

    private void Start() {

        SpawnCar();     //  Spawning the player vehicle.

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 0)
            StartCoroutine(StartRaceDelayed());

    }

    private void Update() {

        //  Adjusting volume smoothly.
        float targetVolume = 1f;

        if (AudioListener.volume < targetVolume && !paused && Time.timeSinceLevelLoad > .5f) {

            if (AudioListener.volume < targetVolume) {

                targetVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
                AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, targetVolume, Time.deltaTime);

            }

        }

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
            CheckNetworkPlayers();

#endif

    }

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON

    /// <summary>
    /// Checks both players and their overtaking distances.
    /// </summary>
    private void CheckNetworkPlayers() {

        if (!gameStarted)
            return;

        if (!player || !player2)
            return;

        overtakingDistance = player.transform.position.z - player2.transform.position.z;
        player.distanceToNextPlayer = overtakingDistance;

        if (overtakingDistance > maxDistance)
            HR_PhotonHandler.Instance.NetworkPlayerWon(player.GetComponent<PhotonView>());

    }

#endif

    /// <summary>
    /// Spawning player car.
    /// </summary>
    private void SpawnCar() {

        bool isMultiplayer = PlayerPrefs.GetInt("Multiplayer", 0) == 0 ? false : true;

        if (!isMultiplayer) {

            player = (RCC.SpawnRCC(HR_PlayerCars.Instance.cars[selectedCarIndex].playerCar.GetComponent<RCC_CarControllerV3>(), spawnLocation.position, spawnLocation.rotation, true, false, true)).GetComponent<HR_PlayerHandler>();
            player.canCrash = true;
            player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, minimumSpeed / 1.75f);
            RCC_Customization.LoadStats(player.GetComponent<RCC_CarControllerV3>());
            StartCoroutine(CheckDayTime());

        } else {

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON

            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
                spawnLocation.transform.position += Vector3.right * 1.25f;
            else
                spawnLocation.transform.position -= Vector3.right * 1.25f;

            player = PhotonNetwork.Instantiate("Photon Player Vehicles/" + HR_PlayerCars.Instance.cars[selectedCarIndex].playerCar.name, spawnLocation.position, spawnLocation.rotation).GetComponent<HR_PlayerHandler>();
            player.canCrash = false;
            RCC.RegisterPlayerVehicle(player.GetComponent<RCC_CarControllerV3>(), false, true);
            player.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, minimumSpeed / 1.75f);
            RCC_Customization.LoadStats(player.GetComponent<RCC_CarControllerV3>());
            StartCoroutine(CheckDayTime());
            HR_PhotonHandler.Instance.NetworkPlayerSpawned(player.gameObject.GetPhotonView());

#endif

        }

        //	Listening event when player spawned.
        if (OnPlayerSpawned != null)
            OnPlayerSpawned(player);

    }

    /// <summary>
    /// Countdown before the game.
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartRaceDelayed() {

        if (OnCountDownStarted != null)
            OnCountDownStarted();

        yield return new WaitForSeconds(4);

        gameStarted = true;
        RCC.SetControl(player.GetComponent<RCC_CarControllerV3>(), true);

        if (OnRaceStarted != null)
            OnRaceStarted();

    }

    /// <summary>
    /// Checking time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckDayTime() {

        yield return new WaitForFixedUpdate();

        if (dayOrNight == DayOrNight.Night)
            player.GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = true;
        else
            player.GetComponent<RCC_CarControllerV3>().lowBeamHeadLightsOn = false;

    }

    /// <summary>
    /// When player crashed.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="scores"></param>
    public void CrashedPlayer(HR_PlayerHandler player, int[] scores) {

        gameStarted = false;

        if (OnPlayerDied != null)
            OnPlayerDied(player, scores);

        StartCoroutine(FinishRaceDelayed(1f));

    }

    /// <summary>
    /// Finished the game after the crash and saves the highscore.
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public IEnumerator FinishRaceDelayed(float delayTime) {

        yield return new WaitForSecondsRealtime(delayTime);
        FinishRace();

    }

    /// <summary>
    /// Finishes the game after the crash and saves the highscore instantly.
    /// </summary>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public void FinishRace() {

        switch (mode) {

            case Mode.OneWay:
                PlayerPrefs.SetInt("bestScoreOneWay", (int)player.score);
                break;
            case Mode.TwoWay:
                PlayerPrefs.SetInt("bestScoreTwoWay", (int)player.score);
                break;
            case Mode.TimeAttack:
                PlayerPrefs.SetInt("bestScoreTimeAttack", (int)player.score);
                break;
            case Mode.Bomb:
                PlayerPrefs.SetInt("bestScoreBomb", (int)player.score);
                break;

        }

    }

    /// <summary>
    /// Main menu.
    /// </summary>
    public void MainMenu() {

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 0) {

            SceneManager.LoadScene(0);

        } else {

#if PHOTON_UNITY_NETWORKING && BCG_HR_PHOTON
            PhotonNetwork.LeaveRoom();
#endif

        }

    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void RestartGame() {

        if (PlayerPrefs.GetInt("Multiplayer", 0) == 1)
            return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    /// <summary>
    /// Pause or resume the game.
    /// </summary>
    public void Paused() {

        paused = !paused;

        if (paused)
            OnPaused();
        else
            OnResumed();

    }

}
