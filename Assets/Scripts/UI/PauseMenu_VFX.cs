using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PauseMenu_VFX : MonoBehaviour
{
    [SerializeField] PauseGame pauseGame;

    [SerializeField] VisualEffect constantBloodEffect;
    [SerializeField] List<VisualEffect> explosionsEffects = new List<VisualEffect>();
    [SerializeField] Animator pentagonAnimator;
    private void OnEnable()
    {
        pauseGame.OnPauseMenu += PauseEffects;
        pauseGame.OnUnpauseMenu += UnpauseEffects;
    }
    void PauseEffects()
    {
        constantBloodEffect.Play(); //play constant blood
        constantBloodEffect.enabled = true;

        pentagonAnimator.SetTrigger("play");

        foreach (VisualEffect efect in explosionsEffects)
        {
            efect.enabled = true;
        }
    }
    void UnpauseEffects()
    {
        constantBloodEffect.Stop();
        constantBloodEffect.enabled = false;
        foreach (VisualEffect efect in explosionsEffects)
        {
            efect.enabled = false;
        }
    }
}
