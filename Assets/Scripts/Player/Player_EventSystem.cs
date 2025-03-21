using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_EventSystem : MonoBehaviour
{
    public Action CallShowAndEnable;
    public Action CallHideAndDisable;


    public Action<UpgradeContainer> OnPickedNewUpgrade;
    public Action<Weapon_InfoHolder> OnPickedNewWeapon;
}
