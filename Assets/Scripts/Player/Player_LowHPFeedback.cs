using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class Player_LowHPFeedback : MonoBehaviour
{
    [Range(0,100)]
    [SerializeField] float PercentageToActivate = 20;
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] float lowPassON_Value = 700, lowPassOFF_Value = 22000, lowPassDEATH_Value = 400;
    [SerializeField] float vignetteON_Value01 = .5f, vignetteON_Value02 = .25f;
     Vignette vignetteEffect;


    [SerializeField] GameObject HP_HolderTf;
    IHealth playerHealth;
    private void OnValidate()
    {
        UsefullMethods.CheckIfGameobjectImplementsInterface<IHealth>(ref HP_HolderTf, ref playerHealth);
    }
    private void Awake()
    {
        Volume vol = GetComponent<Volume>();
        if (vol.profile.TryGet(out Vignette component))
        {
            vignetteEffect = component;
        }
        OnValidate();
    }

    private void OnEnable()
    {
        playerHealth.OnHealthUpdated += CheckHealthPercentage;
    }
    private void OnDisable()
    {
        playerHealth.OnHealthUpdated -= CheckHealthPercentage;
    }
    private void CheckHealthPercentage()
    {
        float minHPForEffect = playerHealth.GetMaxHealth() * (UsefullMethods.normalizePercentage(PercentageToActivate, false, true));

        if(Mathf.Approximately(playerHealth.GetCurrentHealth(),0f))
        {
            TweenLowPass(lowPassDEATH_Value, 0.5f);
            TweenVignetteReturn();
        }
        else if(playerHealth.GetCurrentHealth() <= minHPForEffect)
        {
            TweenLowPass(lowPassON_Value, 2f);
            TweenVignetteYoyo(vignetteON_Value01,1f);
        }
        else
        {
            TweenLowPass(lowPassOFF_Value, 1.5f);
            TweenVignetteReturn();
        }
    }

    #region specific functions
    void TweenLowPass(float value, float time)
    {
        DOTween.To(
            () => GetLowPass(),
            x => SetLowPass(x),
            value,
            time);
    }
    Sequence yoyoSeq;
    void TweenVignetteYoyo(float endValue, float time)
    {
        if (yoyoSeq != null) { return; }
        vignetteEffect.intensity.value = vignetteON_Value02;
        yoyoSeq = DOTween.Sequence();
        yoyoSeq.Append(
        DOTween.To(
            () => vignetteEffect.intensity.value,
            x => vignetteEffect.intensity.value = x,
            endValue,
            time
            ).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo));
    }
    void TweenVignetteReturn()
    {
        if (yoyoSeq != null) { yoyoSeq.Kill();  yoyoSeq = null; }
        DOTween.To(() => vignetteEffect.intensity.value,
            x => vignetteEffect.intensity.value = x,
            0,
            1);
    }
    float GetLowPass()
    {
        if (masterMixer.GetFloat("LowPass", out float x))
        {
            return x;
        }
        return 0f;
    }
    void SetLowPass(float value)
    {
        masterMixer.SetFloat("LowPass", value);
    }
    #endregion
}
