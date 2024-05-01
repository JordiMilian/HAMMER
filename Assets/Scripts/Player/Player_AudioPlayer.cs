using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioPlayer : Generic_CharacterAudioPlayer
{
    [SerializeField] AudioClip RollSFX;
    [SerializeField] AudioClip RebornSFX;
    [SerializeField] AudioClip AttemptParrySFX;
    [SerializeField] Player_EventSystem playerEvents;
    private void OnEnable()
    {
        base.OnEnable();
        playerEvents.OnPerformRoll += playRoll;
        playerEvents.CallEnable += playHeadReatached;
        playerEvents.OnPerformParry += playAttemptParry;
    }
    private void OnDisable()
    {
        base.OnDisable();
        playerEvents.OnPerformRoll -= playRoll;
        playerEvents.CallEnable -= playHeadReatached;
    }
    void playAttemptParry()
    {
        playSFX(AttemptParrySFX, 0.1f,-0.5f,0.5f);
    }
    void playRoll()
    {
        playSFX(RollSFX, 0.1f);
    }
    void playHeadReatached()
    {
        playSFX(RebornSFX);
    }
}
