using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_SfxManger : MonoBehaviour
{
    //I think this is deprecated wtf????
    /*
    [SerializeField] Player_EventSystem playerEvents;
    private void OnEnable()
    {
        Debug.Log("WHERE IS THIS USED????");
        playerEvents.OnAttackStarted += playSwing;
        playerEvents.OnDealtDamage += onHitEnemy;
        playerEvents.OnSuccessfulParry += onsuccesfullParry;
    }
    void playSwing()
    {
        Game_AudioPlayerSingleton.Instance.playSFX("Swing01");
    }
    void onHitEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        Game_AudioPlayerSingleton.Instance.playSFX("HitWood01");
    }
    void onsuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        Game_AudioPlayerSingleton.Instance.playSFX("Parry01");
    }
    */
}
