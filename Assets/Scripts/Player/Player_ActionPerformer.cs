using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_ActionPerformer : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Player_References playerRefs;
    public class Action
    {
        public string triggerName;
        State state;
        public Action(string triggername)
        {
            triggerName = triggername;
        }
    }
    public void AddAction(Action action)
    {
        if (PauseGame.isPaused) { return; }
        //if (playerRefs.disableController.isScriptDisabled) { return; }
        if (!playerAnimator.GetBool("isInputing"))
        {
            Debug.Log("Currently not reading Input, specially not " + action.triggerName);
            return;
        }
        ResetAllTriggers(playerAnimator);
        playerAnimator.SetBool(action.triggerName,true);

    }
    public void EV_returnInput()
    {
        playerAnimator.SetBool("isInputing", true);
    }
    public void EV_canTransition()
    {
        playerAnimator.SetBool("canTransition", true);
    }
    void ResetAllTriggers(Animator animator)
    {
        foreach (var param in animator.parameters)
        {
            if (param.name.StartsWith("Act_"))
            {
                animator.SetBool(param.name, false);
            }
        }
    }
}
