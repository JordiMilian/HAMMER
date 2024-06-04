using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy_ReusableStateMachine : MonoBehaviour
{
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
    [SerializeField] animationReplacer[] animationReplacerArray;
    public enum animationStates
    {
        BaseEnemy_Attacking, BaseEnemy_Parried, BaseEnemy_Agroo, BaseEnemy_Damaged, BaseEnemy_ResponseAttack
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
        statesDictionary.Add(animationStates.BaseEnemy_Damaged, "BaseEnemy_Damaged");
        statesDictionary.Add(animationStates.BaseEnemy_ResponseAttack, "BaseEnemy_ResponseAttack");


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
}
