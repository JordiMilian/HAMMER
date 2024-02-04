using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenBoss_AnimationEvents : MonoBehaviour
{
    [SerializeField] Enemy_AttacksProviderV2 fase01Provider;
    [SerializeField] Enemy_AttacksProviderV2 fase02Provider;
    Enemy_AttacksProviderV2 currentProvider;
    [SerializeField] Enemy_AgrooMovement fase01Movement;
    [SerializeField] Enemy_AgrooMovement fase02Movement;
    Enemy_AgrooMovement currentMovement;
    [SerializeField] GreenBoss_EventSystem eventSystem;

    private void OnEnable()
    {
        eventSystem.OnPhase01 += SwitchPhase01;   
        eventSystem.OnPhase02 += SwitchPhase02;   
    }
    private void OnDisable()
    {
        eventSystem.OnPhase01 -= SwitchPhase01;
        eventSystem.OnPhase02 -= SwitchPhase02;
    }
    void SwitchPhase01(object sender, EventArgs args)
    {
        currentProvider = fase01Provider;
        currentMovement = fase01Movement; 
    }
    void SwitchPhase02(object sender, EventArgs args)
    {
        currentProvider = fase02Provider;
        currentMovement = fase02Movement;
    }
    public void EV_Enemy_ShowAttackCollider()
    {
        currentProvider.EV_Enemy_ShowAttackCollider();
    }
    public void EV_Enemy_HideAttackCollider()
    {
        currentProvider.EV_Enemy_HideAttackCollider();
    }
    public void EV_SlowRotationSpeed() { currentMovement.EV_SlowRotationSpeed(); }
    public void EV_ReturnRotationSpeed() { currentMovement.EV_ReturnRotationSpeed(); }
    public void EV_SlowMovingSpeed() { currentMovement.EV_SlowMovingSpeed(); }
    public void EV_ReturnMovingSpeed() { currentMovement.EV_ReturnMovingSpeed(); }
    public void EV_ReturnAllSpeed() { currentMovement.EV_ReturnAllSpeed(); }
}
