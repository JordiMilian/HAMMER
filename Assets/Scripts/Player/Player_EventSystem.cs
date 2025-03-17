using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : Generic_EventSystem
{
    public Action CallShowAndEnable;
    public Action CallHideAndDisable;
    public Action CallDisable;
    public Action CallEnable;


    public Action<UpgradeContainer> OnPickedNewUpgrade;
    public Action<WeaponPrefab_infoHolder> OnPickedNewWeapon;
}
