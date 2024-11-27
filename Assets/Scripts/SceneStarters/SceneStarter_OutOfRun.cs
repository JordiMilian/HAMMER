using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_OutOfRun : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    [SerializeField] Player_RespawnerManager playerRespawnerManager;
    [SerializeField] FurthestDoor_Manager furthestDoorManager;
    [SerializeField] DeadParts_Manager deadParts_Manager;

    [SerializeField] GameObject roomGeneratorPrefab;
    
    
    
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
        yield return null;
    }
    IEnumerator Initialization() //Cridar les funcions basiques d'estos scripts
    {
        globalPlayerReference.SetPlayerReferences(mainCharacter);
        targetGroupSingleton.Initialize();
        playerRespawnerManager.Initialize();
        
   

        yield return null;
    }
    IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        roomGeneratorPrefab = Instantiate(roomGeneratorPrefab);
        furthestDoorManager.Initialize();
        yield return null;
    }
    IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {
        
        furthestDoorManager.GetAllDoorsAndActivateFurthest();
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        //Spawn player on proper door
        //Logic of currency?
        yield return null;
    }
}
