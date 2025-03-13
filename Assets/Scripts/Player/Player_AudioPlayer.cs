using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioPlayer : Generic_CharacterAudioPlayer
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


        GameEvents.OnPlayerReappear += playHeadReatached;
        playerEvents.OnPerformParry += playAttemptParry;
        playerEvents.OnPickedNewUpgrade += playPickUpgrade;
        playerEvents.OnPickedNewWeapon += playPickUpWeapon;
    }
    public  void OnDisable()
    {
        GameEvents.OnPlayerReappear -= playHeadReatached;
        playerEvents.OnPerformParry += playAttemptParry;
        playerEvents.OnPickedNewUpgrade -= playPickUpgrade;
        playerEvents.OnPickedNewWeapon -= playPickUpWeapon;
    }
    void playAttemptParry()
    {
        SFX_Player.playSFX(AttemptParrySFX, 0.1f,-0.5f,0.5f);
    }
    void playHeadReatached()
    {
        SFX_Player.playSFX(RebornSFX);
    }
    void playPickUpWeapon(WeaponPrefab_infoHolder info)
    {
        SFX_Player.playSFX(PickWeapon);
    }
    void playPickUpgrade(UpgradeContainer upgrade)
    {
        SFX_Player.playSFX(PickUpgrade);
    }

}
