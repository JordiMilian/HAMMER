using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterExitScene_controller : MonoBehaviour
{
    //public Scene sceneToLoad;
    public string SceneName;
    [SerializeField] BaseCutsceneLogic enterCutscene;
    public bool playEnteringCutsceneOnLoad;
    [SerializeField] Transform enterPosition;

    public void EnterPlayerFromHere()
    {
        GlobalPlayerReferences.Instance.playerTf.position = enterPosition.position;
    }
    public void OnPlayerEnteredFromHere()
    {
        CutscenesManager.Instance.AddCutscene(enterCutscene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Tags.Player_SinglePointCollider))
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
