using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy_ReusableStateMachine : MonoBehaviour
{
    //How to create a new reusable state:
        // 1- Add the state name to the Enum animationStates
        // 2- In the Awake, add fill the dictionary with the state name in the Animator
        // 3- In the editor, in the inspector of the enemy whose animation you want to replace,
        //      add a new element selectic in animationReplacerArray with the new enum and the animation to replace

    [SerializeField] RuntimeAnimatorController BaseEnemyController;

    Animator animator;
    AnimatorOverrideController animatorOverrideController;
    AnimationClipOverrides clipOverrides;

    // list of all the basic animations to replace (Not attacks)
    
    [Serializable]
    public class animationReplacer
    {
        public animationStates StateName;
        public AnimationClip Clip;
    }
    public animationReplacer[] animationReplacerArray;
    public enum animationStates
    {
        BaseEnemy_Attacking, BaseEnemy_Parried, BaseEnemy_Agroo, BaseEnemy_Damaged, 
        BaseEnemy_ResponseAttack, BaseEnemy_Parried_Extra, BaseEnemy_Hit, BaseEnemy_BossIntro
    }
    public Dictionary<animationStates,string> statesDictionary = new Dictionary<animationStates,string>();

   
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    private void Awake()
    {
        //On editor the enemy has its own animator controller in order to check the animations.
        //When the game starts it changes to BaseEnemy animator so it works as a clean stateMachine

        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = BaseEnemyController; //Make the animator controller as the baseenemy's

        //ni idea, copiat de internet
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;
        clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(clipOverrides);

        //Fill the dictionary
        statesDictionary.Add(animationStates.BaseEnemy_Attacking, "BaseEnemy_Attacking");
        statesDictionary.Add(animationStates.BaseEnemy_Agroo, "BaseEnemy_Agroo");
        statesDictionary.Add(animationStates.BaseEnemy_Parried, "BaseEnemy_Parried");
        statesDictionary.Add(animationStates.BaseEnemy_Parried_Extra, "BaseEnemy_Parried_Extra");
        statesDictionary.Add(animationStates.BaseEnemy_Damaged, "BaseEnemy_Damaged");
        statesDictionary.Add(animationStates.BaseEnemy_ResponseAttack, "BaseEnemy_ResponseAttack");
        statesDictionary.Add(animationStates.BaseEnemy_Hit, "BaseEnemy_Hit");
        statesDictionary.Add(animationStates.BaseEnemy_BossIntro, "BaseEnemy_BossIntro");
        //statesDictionary.Add(animationStates.BaseEnemy_Walking, "BaseEnemy_Walking");


        //replace the basic animations
        if (animationReplacerArray.Length > 0)
        {
            foreach (animationReplacer replacer in animationReplacerArray)
            {
                if(replacer.Clip == null) { continue; }
                clipOverrides[statesDictionary[replacer.StateName]] = replacer.Clip;
            }
            animatorOverrideController.ApplyOverrides(clipOverrides);
        }
        
    }
    public void ReplaceaStatesClip(string stateName, AnimationClip clipToReplace)
    {
        clipOverrides[stateName] = clipToReplace;
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }
    void ReplaceaMultipleStatesClip(string[] statesName, AnimationClip[] clipsToReplace)
    {
        for (int i = 0; i < statesName.Length; i++)
        {
           clipOverrides[statesName[i]] = clipsToReplace[i];
        }
        animatorOverrideController.ApplyOverrides(clipOverrides);
    }
    //this is used elsewhere to find the parry01 clip
    public AnimationClip getClipInReplacer(animationStates stateName)
    {
        foreach (animationReplacer replacer in animationReplacerArray)
        {
            if (replacer.StateName == animationStates.BaseEnemy_Parried)
            {
                return replacer.Clip;
            }
        }
        Debug.LogWarning("No replacement clip found");
        return null;
    }
}
