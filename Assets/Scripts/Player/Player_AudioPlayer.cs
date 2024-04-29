using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AudioPlayer : Generic_CharacterAudioPlayer
{
    [SerializeField] AudioClip RollSFX;
    [SerializeField] AudioClip RebornSFX;
    [SerializeField] Player_EventSystem playerEvents;
    private void OnEnable()
    {
        base.OnEnable();
        playerEvents.OnPerformRoll += playRoll;
        playerEvents.CallEnable += playHeadReatached;
    }
    private void OnDisable()
    {
        base.OnDisable();
        playerEvents.OnPerformRoll -= playRoll;
        playerEvents.CallEnable -= playHeadReatached;
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
