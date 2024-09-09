using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalDoor_Script : BaseCutsceneLogic
{
    [SerializeField] GameState gameState;
    [SerializeField] Animator doorAnimator;
    [SerializeField] Generic_OnTriggerEnterEvents getNearDoor_collider;
    int skullsThatShouldBeUnlocked;
    private void Awake()
    {
        if (gameState.justDefeatedBoss)
        {
            gameState.SkullsThatShouldBeUnlocked++;
        }
        CheckStateAndUpdateDoor();
    }
    void CheckStateAndUpdateDoor()
    {   
        if(gameState.actuallyUnlockedSkulls == 1) { InstaUnlock01(); }
        else if(gameState.actuallyUnlockedSkulls == 2) { InstaUnlock02(); }
        else if(gameState.actuallyUnlockedSkulls == 3) { InstaUnlock03(); }

        if(gameState.SkullsThatShouldBeUnlocked > gameState.actuallyUnlockedSkulls)
        {
            SubscribeToGetNearCollider();
        }
    }
    
    void SubscribeToGetNearCollider()
    {
        getNearDoor_collider.OnTriggerEntered += QueueCutscene;
    }
    public override void playThisCutscene()
    {
        if(gameState.actuallyUnlockedSkulls == 0)
        {
            currentCutscene = StartCoroutine(UnlockSkull01Cutscene());
        }
        else if( gameState.actuallyUnlockedSkulls == 1)
        {
            currentCutscene = StartCoroutine(UnlockSkull02Cutscene());
        }
        else if(gameState.actuallyUnlockedSkulls == 2)
        {
            currentCutscene = StartCoroutine(UnlockSkull03Cutscene());
        }
        else
        {
            Debug.Log("No door cutscene to play");
        }
    }
    void QueueCutscene(Collider2D collider)
    {

        getNearDoor_collider.OnTriggerEntered -= QueueCutscene;

        CutscenesManager.Instance.AddCutscene(this);
    }
    void IterativeCheck()
    {
        if (gameState.actuallyUnlockedSkulls == 1) { InstaUnlock01(); }
        else if (gameState.actuallyUnlockedSkulls == 2) { InstaUnlock02(); }
        else if (gameState.actuallyUnlockedSkulls == 3) { InstaUnlock03(); }

        if (gameState.SkullsThatShouldBeUnlocked > gameState.actuallyUnlockedSkulls)
        {
            QueueCutscene(new Collider2D());
        }
    }
    void InstaUnlock01()
    {
        doorAnimator.SetTrigger("Insta01");
    }
    void InstaUnlock02()
    {
        doorAnimator.SetTrigger("Insta02");
    }
    void InstaUnlock03()
    {
        doorAnimator.SetTrigger("Insta03");
    }
    IEnumerator UnlockSkull01Cutscene()
    {
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;

        playerEvents.CallDisable();
        doorAnimator.SetTrigger("Unlock01");
        yield return new WaitForSeconds(UsefullMethods.getCurrentAnimationLenght(doorAnimator, 0) + 1);

        gameState.actuallyUnlockedSkulls++;
        if (gameState.SkullsThatShouldBeUnlocked > gameState.actuallyUnlockedSkulls)
        {
            IterativeCheck();
        }

        playerEvents.CallEnable();
        onCutsceneOver?.Invoke();
    }
    IEnumerator UnlockSkull02Cutscene()
    {
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;

        playerEvents.CallDisable();
        doorAnimator.SetTrigger("Unlock02");
        yield return new WaitForSeconds(UsefullMethods.getCurrentAnimationLenght(doorAnimator, 0) + 1);
        gameState.actuallyUnlockedSkulls++;
        if (gameState.SkullsThatShouldBeUnlocked > gameState.actuallyUnlockedSkulls)
        {
            IterativeCheck();
        }
        playerEvents.CallEnable();
        onCutsceneOver?.Invoke();
    }
    IEnumerator UnlockSkull03Cutscene()
    {
        Player_EventSystem playerEvents = GlobalPlayerReferences.Instance.references.events;

        playerEvents.CallDisable();
        doorAnimator.SetTrigger("Unlock03");
        yield return new WaitForSeconds(UsefullMethods.getCurrentAnimationLenght(doorAnimator, 0) + 1);
        gameState.actuallyUnlockedSkulls++;
        if (gameState.SkullsThatShouldBeUnlocked > gameState.actuallyUnlockedSkulls)
        {
            IterativeCheck();
        }
        playerEvents.CallEnable();
        onCutsceneOver?.Invoke(); ;
    }
}
