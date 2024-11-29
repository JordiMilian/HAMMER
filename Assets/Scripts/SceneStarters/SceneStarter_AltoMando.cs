using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_AltoMando : SceneStarter_base
{
    [SerializeField] GameObject roomGeneratorPrefab;
    [SerializeField] GameState gameState;

    public override IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        yield return StartCoroutine(base.Creation());

        roomGeneratorPrefab = Instantiate(roomGeneratorPrefab);
        RoomGenerator_Manager roomManager = roomGeneratorPrefab.GetComponent<RoomGenerator_Manager>();
        roomManager.Call_GenerateAllRoomsFromPosition?.Invoke(Vector2.zero);

    }
    public override IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {
        yield return StartCoroutine(base.Preparation());

        //Spawn in zero or respawn in HUB checkpoint
        if (gameState.isTutorialComplete) 
        {
            Debug.Log("tutorial is complete so spawn in HUB");
            RespawnersManager.Instance.ForceSpawnInIndex(RespawnersManager.Instance.Respawners.Count -1); 

        }
        else { GlobalPlayerReferences.Instance.playerTf.position = Vector2.zero; Debug.Log("tutorial is NOT complete so spawn in zero"); }

        Rooms_FadeInOut_StartingRoomsCheck.Instance.FadeInStartingRoom();

    }
}
