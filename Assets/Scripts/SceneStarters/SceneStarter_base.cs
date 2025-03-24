using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_base : MonoBehaviour
{
    public GameObject mainCharacter; 
    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    CameraZoomController cameraZoomController;

    [Header("Already Instantiated")]
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] LoadingScreenController loadingScreenController;

    public IEnumerator Start()
    {
        loadingScreenController.ShowLoadingScreen();

        Debug.Log("SceneStarter: reached 00");
        loadingScreenController.UpdateLoadingBar(0);

        yield return StartCoroutine(Binding());
        Debug.Log("SceneStarter: reached 01");
        loadingScreenController.UpdateLoadingBar(20);

        yield return StartCoroutine(Initialization());
        Debug.Log("SceneStarter: reached 02");
        loadingScreenController.UpdateLoadingBar(40);

        yield return StartCoroutine(Creation());
        Debug.Log("SceneStarter: reached 03");
        loadingScreenController.UpdateLoadingBar(60);

        yield return StartCoroutine(Preparation());
        Debug.Log("SceneStarter: reached 04");
        loadingScreenController.UpdateLoadingBar(80);

        
        Debug.Log("SceneStarter: reached 05");
        loadingScreenController.UpdateLoadingBar(100);

        loadingScreenController.HideLoadingScreen();

        yield return StartCoroutine(StartPlaying());
        
    }

    public virtual IEnumerator Binding() //Crear els scripts basics necesaris
    {
        globalPlayerReference = Instantiate(globalPlayerReference,transform);

        targetGroupSingleton = Instantiate(targetGroupSingleton, transform);

        cameraZoomController = cinemachineCamera.GetComponent<CameraZoomController>();


        mainCharacter = Instantiate(mainCharacter);
        globalPlayerReference.SetPlayerReferences(mainCharacter);

        yield return null;
    }
    public virtual IEnumerator Initialization() //Cridar les funcions basiques d'estos scripts (buscar referencies principalment)
    {
        targetGroupSingleton.GetTargetGroupReference();
        
        cameraZoomController.SetBaseZoomAndReferences();

        Player_References playerRefs = globalPlayerReference.references;
        playerRefs.stateMachine.ForceChangeState(playerRefs.DisabledState);
        yield return null;
    }
    public virtual IEnumerator Creation() //Crear els objectes necesaris (character, escenaris)
    {
        pauseCanvas = Instantiate(pauseCanvas);
        pauseCanvas.GetComponent<PauseGame>().Unpause();

        yield return null;
    }
    public virtual IEnumerator Preparation() //Colocar els objectes de Creation on toque
    {
        //Look at player and mouse
        TargetGroupSingleton.Instance.ReturnPlayersTarget();
        cinemachineCamera.Follow = targetGroupSingleton.transform;

        //Spawn player on proper door
        yield return null;
    }
    public virtual IEnumerator StartPlaying()
    {
        Player_References playerRefs = globalPlayerReference.references;
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);
        yield return null;
    }
}
