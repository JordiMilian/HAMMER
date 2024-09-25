using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeatedUI_Audio : MonoBehaviour
{
    public void EV_PlayBossDefeatedAudio()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.Play();
    }
}
