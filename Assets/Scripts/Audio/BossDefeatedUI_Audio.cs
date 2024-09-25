using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedUI_Audio : MonoBehaviour
{
    [SerializeField] AudioClip UIAppearSFX;
    public void EV_PlayBossDefeatedAudio()
    {
        SFX_PlayerSingleton.Instance.playSFX(UIAppearSFX);
    }
}
