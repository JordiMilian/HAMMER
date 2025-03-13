using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBeginning_SeudoCode : MonoBehaviour
{
    /*
    public enum GenericEnemyStateTypes
    {
        enemyState_Idle,
        enemyState_Agroo,
        enemyState_Parried,
        enemyState_StanceBroken,
        enemyState_Dead
    }
    public interface IParriable
    {
        public void OnParried();
    }

    public class Enemy_BaseEnemy_Controller : MonoBehaviour , IDamageDealer
    {
        [SerializeField] Enemy_StateMachine enemyStateMachine;
        public  void OnDamageDealt(DealtDamageInfo info)
        {

        }
    }
    public abstract class EnemyState : MonoBehaviour
    {
        protected Animator enemyAnimator;
        protected Enemy_StateMachine enemyStateMachine;

        public void InitializeState(Animator animator, Enemy_StateMachine stateMachine)
        {
            enemyAnimator = animator;
            enemyStateMachine = stateMachine;
        }
        public abstract void OnEnable();
        public abstract void OnDisable();
        public abstract void Update();
    }
    public class EnemyState_Idle : EnemyState
    {
        public AnimationClip animationClip;
        public override void OnEnable()
        {
            enemyAnimator.CrossFade(animationClip.name, 0.1f);
        }
        public override void OnDisable()
        {
        }
        public override void Update()
        {
            //If player detected switch to Agroo state
        }
    }
    public abstract class EnemyState_Attack : EnemyState
    {
        public string ShortDescription;
        public Enemy_AttackRangeDetector rangeDetector;
        [HideInInspector] public BoxCollider2D boxCollider;
        [HideInInspector] public bool isActive;
        public bool notOvniInvertable = false;

        [Header("Stats:")]
        public float Damage;
        public int Probability;
        public float KnockBack;
        [HideInInspector] public float Hitstop;
        [Header("Cooldown")]
        public bool isInCooldown;
        public bool HasCooldown;
        public float CooldownTime;

        public void isInRange() { isActive = true; }
        public void isNotInRange() { isActive = false; }

        public IEnumerator Cooldown()
        {
            isInCooldown = true;
            yield return new WaitForSeconds(CooldownTime); //Needs to change cooldown calculation
            isInCooldown = false;
        }
    }
    public class EnemyState_BasicAnimationAttack : EnemyState_Attack
    {
        [SerializeField] AnimationClip attackAnimationClip;
        Coroutine animationLenghtWaiterCoroutine;
        public override void OnEnable()
        {
            enemyAnimator.CrossFade(attackAnimationClip.name, 0.1f);
            animationLenghtWaiterCoroutine = StartCoroutine(animationLenghtWaiter());
        }
        IEnumerator animationLenghtWaiter()
        {
            float timer = 0;
            while (timer < attackAnimationClip.length)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            onAnimationFinished();
        }
        public override void OnDisable()
        {
            if(animationLenghtWaiterCoroutine != null)
            {
                StopCoroutine(animationLenghtWaiterCoroutine);
            }
        }
        public override void Update()
        {
        }
        void onAnimationFinished()
        {
            enemyStateMachine.ChangeToGenericState(GenericEnemyStateTypes.enemyState_Agroo);
        }
    }
    public class Enemy_StateMachine : MonoBehaviour
    {
        public EnemyState currentState;
        public struct genericStateInfo
        {
            public GenericEnemyStateTypes stateType;
            public EnemyState stateScript;
        }
        //We should manually fill this I feel
        public List<genericStateInfo> genericEnemyStatesList = new List<genericStateInfo>();

        genericStateInfo getStateByType(GenericEnemyStateTypes stateType)
        {
            foreach (genericStateInfo state in genericEnemyStatesList)
            {
                if (state.stateType == stateType)
                {
                    return state;
                }
            }
            return new genericStateInfo();
        }
        void SetNewState(EnemyState enemyState)
        {
            if (currentState != null)
            {
                currentState.gameObject.SetActive(false);
            }
            currentState = enemyState;
            currentState.InitializeState(GetComponent<Animator>(), this);
            currentState.gameObject.SetActive(true);
        }

        public void ChangeToGenericState(GenericEnemyStateTypes newState)
        {
            SetNewState( getStateByType(newState).stateScript);
        }
        public void ChangeToSpecificState(EnemyState enemyState)
        {
            SetNewState(enemyState);
        }
    }

    public class Player_StateMachine : MonoBehaviour
    {

    }
    */
}
