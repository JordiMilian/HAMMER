using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_chargeless : MonoBehaviour
{
    public float CurrentDamage;
    public float BaseDamage;

    [SerializeField] Player_References playerRefs;

    [Header("Adapt to distance to Enemy")]
    [SerializeField] float Force;
    [SerializeField] FloatVariable distanceToEnemy;
    [SerializeField] FloatVariable defaultDistance;
    public float minDistance = 0.2f;
    public float maxDistance = 3f;
    [SerializeField] float minForce = -0.5f;
    [SerializeField] float maxForce = 1f;

    private void OnEnable()
    {
        playerRefs.events.OnPerformAttack += RemoveAttackStamina;
        InputDetector.Instance.OnAttackPressed += onAttackPressed;
    }
    private void OnDisable()
    {
        playerRefs.events.OnPerformAttack -= RemoveAttackStamina;
        InputDetector.Instance.OnAttackPressed -= onAttackPressed;
    }
    void RemoveAttackStamina()
    {
        playerRefs.events.OnStaminaAction?.Invoke(2);
    }


    void onAttackPressed()
    {
        if (playerRefs.currentStamina.Value > 0)
        {
            playerRefs.actionPerformer.AddAction(new Player_ActionPerformer.Action("Act_Attack"));
        }
    }
    public void EV_ShowWeaponCollider() { playerRefs.weaponCollider.enabled = true; playerRefs.playerVFX.EV_ShowTrail(); }
    public void EV_HideWeaponCollider() { playerRefs.weaponCollider.enabled = false; playerRefs.playerVFX.EV_HideTrail(); }
    public void EV_AddForce()
    {
        //Make equivalent between min and max distance to -0,5 and 1 (normalize)
        float equivalent;
        if (distanceToEnemy.Value > maxDistance) { equivalent = CalculateEquivalent(defaultDistance.Value); } //If the player is too far, behave with default 
        else { equivalent = CalculateEquivalent(distanceToEnemy.Value); } // Else calculate with distance

        Vector3 tempForce = playerRefs.followMouse.gameObject.transform.up * Force * equivalent;
        StartCoroutine(UsefullMethods.ApplyForceOverTime(playerRefs._rigidbody, tempForce, 0.1f));
    }
    float CalculateEquivalent(float Distance)
    {
        float inverseF = Mathf.InverseLerp(minDistance, maxDistance, Distance);
        float lerpF = Mathf.Lerp(minForce, maxForce, inverseF);
        return lerpF;
    }
}
 

