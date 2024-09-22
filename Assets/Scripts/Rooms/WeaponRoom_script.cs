using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoom_script : MonoBehaviour
{
    [SerializeField] DoorAnimationController doorAnimationController;
    [SerializeField] WeaponPrefab_infoHolder weaponInfo;

    private void OnEnable()
    {
        weaponInfo.OnPickedUpEvent += (WeaponPrefab_infoHolder info) => doorAnimationController.OpenDoor();
        doorAnimationController.DisableAutoDoorOpener();
    }
    private void OnDisable()
    {
        weaponInfo.OnPickedUpEvent -= (WeaponPrefab_infoHolder info) => doorAnimationController.OpenDoor();
    }

}
