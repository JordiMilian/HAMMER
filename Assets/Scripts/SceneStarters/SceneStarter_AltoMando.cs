using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_AltoMando : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    [SerializeField] Player_RespawnerManager playerRespawnerManager;
    [SerializeField] DeadParts_Manager deadParts_Manager;
    CameraZoomController cameraZoomController;

    [SerializeField] GameObject roomGeneratorPrefab;

    [Header("Already Instantiated")]
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;


    IEnumerator Start()
    {
        yield return StartCoroutine(Binding());
        yield return StartCoroutine(Initialization());
        yield return StartCoroutine(Creation());
        yield return StartCoroutine(Preparation());

    }
    IEnumerator Binding() //Crear els scripts basics necesaris
    {
        globalPlayerReference = Instantiate(globalPlayerReference, transform);

        targetGroupSingleton = Instantiate(targetGroupSingleton, transform);
        targetGroupSingleton.AddComponent<CinemachineTargetGroup>();

        playerRespawnerManager = Instantiate(playerRespawnerManager, transform);
        deadParts_Manager = Instantiate(deadParts_Manager, transform);
        cameraZoomController = cinemachineCamera.GetComponent<CameraZoomController>();

        mainCharacter = Instantiate(mainCharacter);
        globalPlayerReference.SetPlayerReferences(mainCharacter);

        yield return null;
    }
    IEnumerator Initialization() //Cridar les funcions basiques d'estos scripts
    {
        targetGroupSingleton.GetTargetGroupReference();
        playerRespawnerManager.Initialize();
        cameraZoomController.Initialize();

        yield return null;
    }
    IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        pauseCanvas = Instantiate(pauseCanvas);
        pauseCanvas.GetComponent<PauseGame>().Unpause();

        roomGeneratorPrefab = Instantiate(roomGeneratorPrefab);
        RoomGenerator_Manager roomManager = roomGeneratorPrefab.GetComponent<RoomGenerator_Manager>();
        roomManager.Call_GenerateAllRoomsFromPosition?.Invoke(Vector2.zero);

        yield return null;
    }
    IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {

        //Look at player and mouse
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        cinemachineCamera.Follow = targetGroupSingleton.transform;

        GlobalPlayerReferences.Instance.playerTf.position = Vector2.zero; //Spawn in zero or in HUB or in front of the 4 doors? maybe potser no cal
        yield return null;
    }
}
