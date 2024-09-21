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
    public override void OnEnable()
    {
        base.OnEnable();
        SFX_Player = SFX_PlayerSingleton.Instance;

        playerEvents.OnPerformRoll += playRoll;
        GameEvents.OnPlayerRespawned += playHeadReatached;
        playerEvents.OnPerformParry += playAttemptParry;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        playerEvents.OnPerformRoll -= playRoll;
        GameEvents.OnPlayerRespawned -= playHeadReatached;
        playerEvents.OnPerformParry += playAttemptParry;
    }
    void playAttemptParry()
    {
        SFX_Player.playSFX(AttemptParrySFX, 0.1f,-0.5f,0.5f);
    }
    void playRoll()
    {
        SFX_Player.playSFX(RollSFX, 0.1f);
    }
    void playHeadReatached()
    {
        SFX_Player.playSFX(RebornSFX);
    }
}
