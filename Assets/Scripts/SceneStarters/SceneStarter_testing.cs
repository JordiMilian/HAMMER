using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_testing : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    [SerializeField] Player_RespawnerManager playerRespawnerManager;
    [SerializeField] FurthestDoor_Manager furthestDoorManager;
    [SerializeField] DeadParts_Manager deadParts_Manager;

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
        globalPlayerReference = Instantiate(globalPlayerReference,transform);

        targetGroupSingleton = Instantiate(targetGroupSingleton, transform);
        targetGroupSingleton.AddComponent<CinemachineTargetGroup>();

        playerRespawnerManager = Instantiate(playerRespawnerManager,transform);
        furthestDoorManager = Instantiate(furthestDoorManager,transform);
        deadParts_Manager = Instantiate(deadParts_Manager,transform);

        
        yield return null;

        mainCharacter = Instantiate(mainCharacter);
        globalPlayerReference.SetPlayerReferences(mainCharacter);
        yield return null;
    }
    IEnumerator Initialization() //Cridar les funcions basiques d'estos scripts
    {
        targetGroupSingleton.GetTargetGroupReference();
        playerRespawnerManager.Initialize();

        yield return null;
    }
    IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        pauseCanvas = Instantiate(pauseCanvas);
        pauseCanvas.GetComponent<PauseGame>().Unpause();

        roomGeneratorPrefab = Instantiate(roomGeneratorPrefab);
        furthestDoorManager.GetGeneratorReference();
        yield return null;
    }
    IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {
        
        furthestDoorManager.GetAllDoorsAndActivateFurthest();

        //Look at player and mouse
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        cinemachineCamera.Follow = targetGroupSingleton.transform;

        //Spawn player on proper door
        //Logic of currency?
        yield return null;
    }
}
