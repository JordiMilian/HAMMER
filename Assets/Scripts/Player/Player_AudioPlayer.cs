using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioClip RollSFX;
    [SerializeField] AudioClip RebornSFX;
    [SerializeField] AudioClip AttemptParrySFX;
    [SerializeField] AudioClip PickWeapon, PickUpgrade; 

    [SerializeField] Player_EventSystem playerEvents;
    SFX_PlayerSingleton SFX_Player;
    public  void OnEnable()
    {
        
        SFX_Player = SFX_PlayerSingleton.Instance;
        GameEvents.OnPlayerRespawned += playHeadReatached;
        playerEvents.OnPickedNewWeapon += playPickUpWeapon;
    }
    public  void OnDisable()
    {
        GameEvents.OnPlayerRespawned -= playHeadReatached;

        playerEvents.OnPickedNewWeapon -= playPickUpWeapon;
    }
    void playHeadReatached()
    {
        SFX_Player.playSFX(RebornSFX);
    }
    void playPickUpWeapon(Weapon_InfoHolder info)
    {
        SFX_Player.playSFX(PickWeapon);
    }

}
