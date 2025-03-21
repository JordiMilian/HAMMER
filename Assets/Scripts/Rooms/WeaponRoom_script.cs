using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRoom_script : MonoBehaviour
{
    [SerializeField] DoorAnimationController doorAnimationController;
    [SerializeField] WeaponPickable_Controller weaponInfo;

    private void OnEnable()
    {
        weaponInfo.OnPickedUpEvent += (WeaponPickable_Controller info) => doorAnimationController.OpenDoor();
        doorAnimationController.DisableAutoDoorOpener();
    }
    private void OnDisable()
    {
        weaponInfo.OnPickedUpEvent -= (WeaponPickable_Controller info) => doorAnimationController.OpenDoor();
    }

}
