using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WeaponSwitcher : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [SerializeField] PolygonCollider2D playersWeaponCollider;
    [SerializeField] Player_ComboSystem_chargeless comboSystem;
    [SerializeField] Player_SpecialAttack specialAttack;
    [SerializeField] Animator playersAnimator;
    [SerializeField] SpriteRenderer playersWeaponSpriteRenderer;
    private void Awake()
    {
        SetNewWeapon(gameState.PlayersWeaponPrefab);
    }
    public void SetNewWeapon(GameObject newWeaponPrefab)
    {
        WeaponPrefab_infoHolder infoHolder = newWeaponPrefab.GetComponent<WeaponPrefab_infoHolder>();

        CopyPolygonCollider(infoHolder.weaponsCollider, playersWeaponCollider);

        comboSystem.Base_Damage = infoHolder.BaseDamage;
        comboSystem.Base_Knockback = infoHolder.BaseKnockback;
        comboSystem.Base_HitStop = infoHolder.BaseHitstop;

        specialAttack.Sp_Damage = infoHolder.Sp_Damage;
        specialAttack.Sp_Knockback = infoHolder.Sp_Knockback;
        specialAttack.StaminaCost = infoHolder.Sp_StaminaCost;

        comboSystem.StaminaUse = infoHolder.StaminaUsePerSwing;

        playersAnimator.runtimeAnimatorController = infoHolder.animatorController;

        playersWeaponSpriteRenderer.sprite = infoHolder.weaponSprite;
    }
    void CopyPolygonCollider(PolygonCollider2D sourceCollider, PolygonCollider2D targetCollider)
    {
        targetCollider.pathCount = sourceCollider.pathCount;
        for (int i = 0; i < sourceCollider.pathCount; i++)
        {
            targetCollider.SetPath(i, sourceCollider.GetPath(i));
        }
        targetCollider.offset = sourceCollider.offset;
        targetCollider.isTrigger = sourceCollider.isTrigger;
    }
}
