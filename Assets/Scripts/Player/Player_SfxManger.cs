using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SfxManger : MonoBehaviour
{
    [SerializeField] Player_EventSystem playerEvents;
    private void OnEnable()
    {
        playerEvents.OnPerformAttack += playSwing;
        playerEvents.OnDealtDamage += onHitEnemy;
        playerEvents.OnSuccessfulParry += onsuccesfullParry;
    }
    void playSwing()
    {
        AudioPlayer.Instance.playSFX("Swing01");
    }
    void onHitEnemy(object sender, Generic_EventSystem.DealtDamageInfo info)
    {
        AudioPlayer.Instance.playSFX("HitWood01");
    }
    void onsuccesfullParry(object sender, Generic_EventSystem.SuccesfulParryInfo info)
    {
        AudioPlayer.Instance.playSFX("Parry01");
    }
}
