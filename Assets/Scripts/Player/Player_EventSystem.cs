using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    
    public Action CallRespawnToLastRespawner;
    public Action CallRespawnToRestartGame;
    public Action CallShowAndEnable;
    public Action CallHideAndDisable;
    public Action CallDisable;
    public Action CallEnable;


    public Action OnAttackStarted; //Called when an attack animation state is entered
    //Attack over is a OnAttackFinished from Generic
    public Action OnPerformParry;
    public Action<GameObject> OnFocusEnemy;
    public Action OnUnfocusEnemy;
    public Action OnPerformSpecialAttack;
    public Action<float> CallStaminaAction;
    public Action<UpgradeContainer> OnPickedNewUpgrade;
    public Action OnRemovedUpgrade;
    public Action OnActuallySpecialHeal;
    public Action<WeaponPrefab_infoHolder> OnPickedNewWeapon;
}
