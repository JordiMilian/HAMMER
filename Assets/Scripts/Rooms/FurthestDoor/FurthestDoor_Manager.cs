using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurthestDoor_Manager : MonoBehaviour
{
    public List<EnterExitScene_withDistance> enterExitScenesList;
    [SerializeField] GameState gameState;
    [SerializeField] RoomGenerator_Manager roomGenerator;

    public static FurthestDoor_Manager Instance;
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
    public void GetGeneratorReference()
    {
        roomGenerator = RoomGenerator_Manager.Instance;
        gameState.LastEnteredDoor = RoomGenerator_Manager.Instance.AreaIndex - 1; 
        
    }
    public void GetAllDoorsAndActivateFurthest() //Called from sceneStarter
    {
        SubscribeToDoors();
        sortDoorsByDistance();
        ActivateGameStatesDoor();
    }
    void SubscribeToDoors()
    {
        foreach (EnterExitScene_withDistance enterExit in enterExitScenesList)
        {
            enterExit.onDoorActivated += UpdateFurthestDoorInState;
        }
    }
    void sortDoorsByDistance()
    {
        SetDistancesToDoors();

        int doorsLenght = enterExitScenesList.Count;
        for (int i = 0; i < doorsLenght; i++)
        {
            int closestIndex = i;
            for (int j = i + 1; j < doorsLenght; j++)
            {
                if (enterExitScenesList[j].DistanceToManager < enterExitScenesList[closestIndex].DistanceToManager)
                {
                    closestIndex = j;
                }
            }

            EnterExitScene_withDistance tempEnterExit = enterExitScenesList[closestIndex];
            enterExitScenesList[closestIndex] = enterExitScenesList[i];
            enterExitScenesList[i] = tempEnterExit;
        }
    }
    void ActivateGameStatesDoor()
    {
        if (gameState.FurthestDoorsArray[roomGenerator.AreaIndex] < 0) { return; }
        foreach (EnterExitScene_withDistance enterExitScene in enterExitScenesList)
        {
            enterExitScene.playEnteringCutsceneOnLoad = false;
        }
        enterExitScenesList[gameState.FurthestDoorsArray[roomGenerator.AreaIndex]].playEnteringCutsceneOnLoad = true;
        enterExitScenesList[gameState.FurthestDoorsArray[roomGenerator.AreaIndex]].tiedEnemyRespawner.ActivateRespawner();
        //Player_RespawnerManager.Instance.Respawners[roomGenerator.AreaIndex].ExternallyActivateRespawner();
    }
    
    void SetDistancesToDoors()
    {
        foreach (EnterExitScene_withDistance enterExit in enterExitScenesList)
        {
            enterExit.DistanceToManager = (enterExit.transform.position - transform.position).sqrMagnitude;
        }
    }
    
    void UpdateFurthestDoorInState()
    {
        gameState.FurthestDoorsArray[roomGenerator.AreaIndex] = GetFurthestActiveDoor();
    }
    int GetFurthestActiveDoor()
    {
        int furthestIndex = 0;
        for (int i = 0; i < enterExitScenesList.Count; i++)
        {
            if (enterExitScenesList[i].DistanceToManager > enterExitScenesList[furthestIndex].DistanceToManager && enterExitScenesList[i].isDoorActive)
            {
                furthestIndex = i;
            }
        }
        return furthestIndex;
    }
}
