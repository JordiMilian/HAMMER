using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_ChargingAttack : EnemyState_Attack
{
    [SerializeField] float minChargingTime = 1;
    [SerializeField] float maxChargingTime = 5;
    [SerializeField] float maxDamage = 10;
    [SerializeField] float maxKnockBack = 3;
    [SerializeField] float distanceToReleaseCharge = 2;

    Coroutine chargingCoroutine;
    public override void OnEnable()
    {
        base.OnEnable();
        chargingCoroutine = StartCoroutine(chargeCoroutine());
    }
    IEnumerator chargeCoroutine()
    {
        EnemyRefs.moveToTarget.SetMovementSpeed(SpeedsEnum.Fast);
        animator.SetTrigger("StartCharge");
        yield return new WaitForSeconds(minChargingTime);

        Transform playerTf = GlobalPlayerReferences.Instance.playerTf;
        
        float startingDamage = EnemyRefs.DamageDealersList[0].Damage;
        float startingKnockBack = EnemyRefs.DamageDealersList[0].Knockback;

        float timer = minChargingTime;
        while (timer < maxChargingTime)
        {
            timer += Time.deltaTime;
            float distanceToPlayer = Vector2.Distance(EnemyRefs.transform.position, playerTf.position);

            foreach(Generic_DamageDealer dealer in EnemyRefs.DamageDealersList)
            {
                dealer.Damage = Mathf.Lerp(startingDamage, maxDamage, timer / maxChargingTime);
                dealer.Knockback = Mathf.Lerp(startingKnockBack, maxKnockBack, timer / maxChargingTime);
            }

            if (distanceToPlayer < distanceToReleaseCharge) 
            {
                yield return release();
                break; 
            }
            yield return null;
        }
        yield return release();


        //
        IEnumerator release()
        {
            animator.SetTrigger("ReleaseCharge");
            yield return WaitForNextAnimationToFinish();
            OnAttackFinished();
        }

    }


    public override void OnDisable()
    {
        base.OnDisable();
        if(chargingCoroutine != null) { StopCoroutine(chargingCoroutine);}
    }
}
