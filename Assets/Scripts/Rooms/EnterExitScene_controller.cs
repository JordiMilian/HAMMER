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

    private void Start()
    {
        if(playEnteringCutsceneOnLoad)
        {
            Invoke( "OnPlayerEnteredFromHere",0.1f);
        }
    }
    public void OnPlayerEnteredFromHere()
    {
        CutscenesManager.Instance.AddCutscene(enterCutscene);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TagsCollection.Player_SinglePointCollider))
        {
            LoadScene();
        }
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
