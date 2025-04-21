using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour
{
    [Header("References")]
    public GameObject mainCharacter;
    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    Player_References playerRefs;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    CameraZoomController cameraZoomController;

    [Header("Already Instantiated")]
    [SerializeField] Cinemachine.CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] LoadingScreenController loadingScreenController;
    [SerializeField] RoomsLoader roomsLoader;

    GameControllerStates currentState = GameControllerStates.None;
    [SerializeField] PlayModes playMode;
    [SerializeField] GameObject GO_TestingRoom;

    [Header("Testing rooms")]
    [SerializeField] int indexOfTestingRoom = 0;
    [SerializeField] List<GameObject> TestingRoomsList = new List<GameObject>();

    public enum PlayModes
    {
        RoomTestingMode, GameMode
    }
    public enum GameControllerStates
    {
        None, MainMenu, Playing, Loading, Paused, ChoosingRoomMenu
    }
    #region AWAKE
    private void Awake()
    {
        GetReferencesOnAwake();

        switch (playMode)
        {
            case PlayModes.GameMode:
                //load main menu
                break;
            case PlayModes.RoomTestingMode:
                //load testing room
                roomsLoader.LoadNewRoom(GO_TestingRoom);
                ChangeGameControllerState(GameControllerStates.Playing);
                globalPlayerReference.references.stateMachine.ForceChangeState(globalPlayerReference.references.EnteringRoomState);
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

            pauseCanvas = Instantiate(pauseCanvas);
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
                break;
            case GameControllerStates.Loading:
                //close loading UI
                break;
            case GameControllerStates.Paused:
                //close paused UI
                break;
        }

        switch (newState)
        {
            case GameControllerStates.MainMenu:
                //load menu UI
                break;
            case GameControllerStates.Playing:
                //return control to player
                break;
            case GameControllerStates.Loading:
                //open loading screen
                break;
            case GameControllerStates.Paused:
                //open pause UI
                break;
        }
        currentState = newState;
    }
    public void OnExitedRoom()
    {
        //For testing now, this should be replaced with opening the MAP
        //
        ChangeGameControllerState(GameControllerStates.Loading);
        indexOfTestingRoom++;
        roomsLoader.LoadNewRoom(TestingRoomsList[indexOfTestingRoom]);
        ChangeGameControllerState(GameControllerStates.Playing);
        playerRefs.stateMachine.ForceChangeState(playerRefs.EnteringRoomState);
    }
    public void RespawnToLastCheckpoint()
    {

    }
    public void ReturnToAltoMando()
    {

    }
    
}

