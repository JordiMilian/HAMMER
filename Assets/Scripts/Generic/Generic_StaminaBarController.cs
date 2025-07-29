using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generic_StaminaBarController : MonoBehaviour
{
    [SerializeField] PlayerStats currentStats;
    [SerializeField] Player_Stamina stamina;

    [SerializeField] Transform Bar_Tf;
    [SerializeField] Transform Bg_Tf;

    [SerializeField] float HorizontalSizePerUnit = .5f;

    Vector3 Bar_BaseScale;
    Vector3 Bg_BaseScale;
    [SerializeField] float Bg_HorizontalOffset;

    private void Update()
    {
        if(!stamina.isFilled)
        {
            UpdateBarSize(currentStats.CurrentStamina);
        }
        
    }
    private void OnEnable()
    {
        currentStats.OnMaxStaminaChange += UpdateBgSize;

        Bar_BaseScale = Bar_Tf.localScale;
        Bg_BaseScale = Bg_Tf.localScale;

        UpdateBarSize(currentStats.CurrentStamina);
        UpdateBgSize(currentStats.MaxStamina);
    }
    private void OnDisable()
    {
        currentStats.OnMaxStaminaChange -= UpdateBgSize;
    }
    void UpdateBarSize(float currentStamina)
    {
        Bar_Tf.localScale = new Vector3(currentStamina * HorizontalSizePerUnit, Bar_BaseScale.y, Bar_BaseScale.z);
    }

    void UpdateBgSize(float newMaxStamina)
    {
        Bg_Tf.localScale = new Vector3((newMaxStamina * HorizontalSizePerUnit) + Bg_HorizontalOffset, Bg_BaseScale.y, Bg_BaseScale.z);
    }
    [SerializeField] SpriteRenderer flashSprite;
    Sequence seq;
    public void NotEnoughStaminaFeedback()
    {
        CameraShake.Instance.ShakeCamera(IntensitiesEnum.Small);
        if (seq == null)
        {
            seq = DOTween.Sequence();
            seq.Append(DOTween.To(() => flashSprite.color.a, x => flashSprite.color = new Color(1, 1, 1, x), .35f, .1f));
            seq.Append(DOTween.To(() => flashSprite.color.a, x => flashSprite.color = new Color(1, 1, 1, x), 0, .25f));
            seq.onComplete += () => seq = null;
        }
    }
}
