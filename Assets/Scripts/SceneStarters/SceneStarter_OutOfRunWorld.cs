using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStarter_OutOfRunWorld : SceneStarter_base
{
    [SerializeField] GameState gameState;
    [SerializeField] Animator UITransitionAnimator;
    public override IEnumerator Preparation()
    {
        yield return StartCoroutine( base.Preparation());
        TransformLevelsToCurrency();

    }
    void TransformLevelsToCurrency()
    {
        //Transform currencies and play animation
        //Reset upgrades and stuff
        //Reset XP
    }
}
