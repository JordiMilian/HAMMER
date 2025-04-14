using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterExitScene_controller : MonoBehaviour, ICutsceneable
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
        CutscenesManager.Instance.AddCutsceneable(this);
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
    #region CUTSCENE
    [SerializeField] Transform enteringPos;

    public IEnumerator ThisCutscene()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        Player_StateMachine playerStateMachine = playerRefs.stateMachine;
        Transform playerTf = playerRefs.gameObject.transform;

        playerStateMachine.ForceChangeState(playerRefs.DisabledState);
        playerTf.position = enteringPos.position;

        //playerRefs.animator.SetTrigger("EnterRoom Lo que sigue etc"); 

        yield return new WaitForSeconds(.5f); // esperar a que acabe l'animacio

        playerStateMachine.ForceChangeState(playerRefs.IdleState);

    }
    public void ForceEndCutscene()
    {
        Player_References playerRefs = GlobalPlayerReferences.Instance.references;
        playerRefs.transform.position = enteringPos.position;
        playerRefs.stateMachine.ForceChangeState(playerRefs.IdleState);

    }
    #endregion
}
