using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_AltoMandoArea : SceneStarter_base
{

    [SerializeField] GameObject roomGeneratorPrefab;

    IEnumerator Start()
    {
        yield return StartCoroutine(Binding());
        Debug.Log("1");
        yield return StartCoroutine(Initialization());
        Debug.Log("2");
        yield return StartCoroutine(Creation());
        Debug.Log("3");
        yield return StartCoroutine(Preparation());
        Debug.Log("4");

    }
    public override IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        yield return StartCoroutine(base.Creation());

        roomGeneratorPrefab = Instantiate(roomGeneratorPrefab);
        RoomGenerator_Manager roomManager = roomGeneratorPrefab.GetComponent<RoomGenerator_Manager>();
        roomManager.Call_GenerateAllRoomsFromPosition?.Invoke(Vector2.zero);
        
    }
    public override IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {
        yield return StartCoroutine(base.Creation());

        

        GlobalPlayerReferences.Instance.playerTf.position = Vector2.zero; //feo. Implementar amb alguna animacio porfi

        Rooms_FadeInOut_StartingRoomsCheck.Instance.FadeInStartingRoom();
    }
}
