using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneStarter_base : MonoBehaviour
{
    [SerializeField] GameObject mainCharacter;
    [SerializeField] GameObject pauseCanvas;

    [SerializeField] GlobalPlayerReferences globalPlayerReference;
    [SerializeField] TargetGroupSingleton targetGroupSingleton;
    CameraZoomController cameraZoomController;

    [Header("Already Instantiated")]
    [SerializeField] CinemachineVirtualCamera cinemachineCamera;

    IEnumerator Start()
    {
        yield return StartCoroutine(Binding());
        yield return StartCoroutine(Initialization());
        yield return StartCoroutine(Creation());
        yield return StartCoroutine(Preparation());
        
    }
    public virtual IEnumerator Binding() //Crear els scripts basics necesaris
    {
        globalPlayerReference = Instantiate(globalPlayerReference,transform);

        targetGroupSingleton = Instantiate(targetGroupSingleton, transform);
        targetGroupSingleton.AddComponent<CinemachineTargetGroup>();

        cameraZoomController = cinemachineCamera.GetComponent<CameraZoomController>();


        mainCharacter = Instantiate(mainCharacter);
        globalPlayerReference.SetPlayerReferences(mainCharacter);
        yield return null;
    }
    public virtual IEnumerator Initialization() //Cridar les funcions basiques d'estos scripts
    {
        targetGroupSingleton.GetTargetGroupReference();
        cameraZoomController.SetBaseZoomAndReferences();
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
        //Logic of currency?
        yield return null;
    }
}
