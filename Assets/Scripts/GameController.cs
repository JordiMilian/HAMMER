using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    [Header("References")]
    public GameObject mainCharacter;
    

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    Player_References playerRefs;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    CameraZoomController cameraZoomController;

    [Header("Already Instantiated")]
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachineCamera;
    
    public RoomsLoader roomsLoader;

    GameControllerStates currentState = GameControllerStates.None;
    [SerializeField] PlayModes playMode;

    [Header("Menus")]
    [SerializeField] LoadingScreenController loadingScreenController;
    [SerializeField] PauseGame pauseGame;

    [Header("Rooms Loading")]
    [SerializeField] int indexOfLastEnteredRoom = 0;
    [SerializeField] List<GameObject> CurrentRoomsToLoadList = new List<GameObject>(); //This list has to be modified when entering AltoMando door
    [SerializeField] GameObject RespawnRoom;
    [SerializeField] GameObject AltoMandoRoom;

    #region Singleton Logic
    public static GameController Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    public enum PlayModes
    {
        RoomTestingMode, GameMode
    }
    public enum GameControllerStates
    {
        None, MainMenu, Playing, Paused, ChoosingRoomMenu
    }
    #region AWAKE
    private IEnumerator Start()
    {
        GetReferencesOnAwake();

        switch (playMode)
        {
            case PlayModes.GameMode:
                //load main menu
                break;
            case PlayModes.RoomTestingMode:
                //load testing room
                yield return StartCoroutine(roomsLoader.LoadNewRoom(CurrentRoomsToLoadList[indexOfLastEnteredRoom]));
                ChangeGameControllerState(GameControllerStates.Playing);
                playerRefs.stateMachine.ForceChangeState(playerRefs.EnteringRoomState);
                break;
            default:
                //ERROR PLS ADD A VALID MODE
                break;
        }

        //
        void GetReferencesOnAwake()
        {
            globalPlayerReference = Instantiate(globalPlayerReference, transform);
            targetGroupSingleton = Instantiate(targetGroupSingleton, transform);
            cameraZoomController = cinemachineCamera.GetComponent<CameraZoomController>();
            mainCharacter = Instantiate(mainCharacter);
            globalPlayerReference.SetPlayerReferences(mainCharacter);
            playerRefs = globalPlayerReference.references;

            targetGroupSingleton.GetTargetGroupReference();
            cameraZoomController.SetBaseZoomAndReferences();
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);

            pauseGame = Instantiate(pauseGame);

            cinemachineCamera.Follow = targetGroupSingleton.transform;
        }
    }
    #endregion
    public void ChangeGameControllerState(GameControllerStates newState)
    {
        switch (currentState)
        {
            case GameControllerStates.None:
                break;
            case GameControllerStates.MainMenu:
                //close menu UI
                break;
            case GameControllerStates.Playing:
                //stop playing logic (pause game, remove input)
                pauseGame.PauseGame_();
                break;
            case GameControllerStates.Paused:
                //close paused UI
                pauseGame.Unpause_andHidePauseUI();
                break;
        }

        switch (newState)
        {
            case GameControllerStates.MainMenu:
                //load menu UI
                break;
            case GameControllerStates.Playing:
                //return control to player
                pauseGame.UnpauseGame();
                break;
            case GameControllerStates.Paused:
                //open pause UI
                pauseGame.Pause_andShowPauseUI();
                break;
        }
        currentState = newState;
    }
    public void OnExitedRegularRoom()
    {
        if(indexOfLastEnteredRoom == CurrentRoomsToLoadList.Count - 1)
        {
            StartCoroutine(loadAltoMando_Coroutine());

            return;
        }
        indexOfLastEnteredRoom++;
        StartCoroutine(loadNextRoomCoroutine());

        //
        IEnumerator loadNextRoomCoroutine()
        {
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
            yield return roomsLoader.LoadNewRoom(CurrentRoomsToLoadList[indexOfLastEnteredRoom]);
            ChangeGameControllerState(GameControllerStates.Playing);
            playerRefs.stateMachine.ForceChangeState(playerRefs.EnteringRoomState);
        }

        IEnumerator loadAltoMando_Coroutine()
        {
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
            yield return roomsLoader.LoadNewRoom(AltoMandoRoom);
            playerRefs.stateMachine.ForceChangeState(playerRefs.RespawningState);
        }
    }
    public void ReloadCurrentRoom()
    {
        StartCoroutine(reloadCurrentRoom_Coroutine());
        //
        IEnumerator reloadCurrentRoom_Coroutine()
        {
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
            yield return roomsLoader.LoadNewRoom(CurrentRoomsToLoadList[indexOfLastEnteredRoom]);
            playerRefs.stateMachine.ForceChangeState(playerRefs.EnteringRoomState);
        }
    }
    public IEnumerator LoadCheckPointRoom()
    {
        indexOfLastEnteredRoom--;
        yield return roomsLoader.LoadNewRoom(RespawnRoom);
    }
    public void OnExitedArea()//Called when you leave an area throw the last door
    {
        StartCoroutine(exitedArea_coroutine());
        IEnumerator exitedArea_coroutine()
        {
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
            yield return roomsLoader.LoadNewRoom(AltoMandoRoom);
            playerRefs.stateMachine.ForceChangeState(playerRefs.RespawningState);
        }
    }
    internal void OnEnteredNewAreaDoor(List<GameObject> newRooms)
    {
        CurrentRoomsToLoadList = new() { RespawnRoom};
        CurrentRoomsToLoadList.AddRange(newRooms);
        indexOfLastEnteredRoom = 0;
        StartCoroutine(enteredNewArea_Coroutine());
        
        IEnumerator enteredNewArea_Coroutine()
        {
            playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
            yield return roomsLoader.LoadNewRoom(RespawnRoom);
            playerRefs.stateMachine.ForceChangeState(playerRefs.EnteringRoomState);
        }
    }
}

