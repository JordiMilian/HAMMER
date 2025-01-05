using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy_StateController_BossEnemy : Enemy_StateController_BasicEnemy
{
    [SerializeField] AnimationClip bossIntroAnimationClip;

    public IEnumerator BossIntroAnimation()
    {
        enemyRefs.animator.SetTrigger(Tags.BossIntro);
        yield return new WaitForSeconds(bossIntroAnimationClip.length);
    }
}
