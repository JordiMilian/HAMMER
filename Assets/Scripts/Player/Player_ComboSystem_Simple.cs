using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_ComboSystem_Simple : MonoBehaviour
{
    [SerializeField] float CheckIfHoldTime = 0.2f;
    [SerializeField] float AttackTime = 1;
    [SerializeField] float BaseDamage;
   
    public int ComboCue;
    string Attack01_Charging = "Attack01_Charging", Attack02_Charging = "Attack02_Charging", Attack01_Release = "Attack01_Release", Attack02_Release = "Attack02_Release";
    private enum ComboState
    {
        Attackin01, Attackin02, NoAttacking, TotalRecovery01, TotalRecovery02, Charging01, Charging02, ActuallyCharging01, ActuallyCharging02,
    }
    [SerializeField] ComboState comboState = ComboState.NoAttacking;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(comboState);
        if (ComboCue <= 2)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                ComboCue++;
                StartCoroutine(CheckIfHold());
                SetToCharge();
            }

            if (Input.GetKeyUp(KeyCode.N))
            {
                Debug.Log("Released");
                SetToRelease();

            }  
        }
        

    }
    void SetToRelease()
    {
        switch (comboState)
        {
            case ComboState.ActuallyCharging01:
                comboState = ComboState.Attackin01;
                animator.SetBool(Attack01_Release, true);
                
                break;

            case ComboState.ActuallyCharging02:
                comboState = ComboState.Attackin02;
                animator.SetBool(Attack02_Release, true);
                
                break;

            case ComboState.Charging01:
                comboState = ComboState.Attackin01;
                animator.SetBool(Attack01_Release, true);
               
                break;

            case ComboState.Charging02:
                comboState = ComboState.Attackin02;
                animator.SetBool(Attack02_Release, true);
                
                break;

            default:
                Debug.Log(comboState + "¿?¿");
                break;
        }
    }
    void SetToCharge()
    {
        switch (comboState)
        {
            default:
                Debug.Log(comboState + "¿?");
                break;
            case ComboState.Attackin02:
                comboState = ComboState.Charging01;
                animator.SetBool(Attack01_Charging, true);

                break;
            case ComboState.Attackin01:
                comboState = ComboState.Charging02;
                animator.SetBool(Attack02_Charging, true);
                break;
            case ComboState.NoAttacking:
                comboState = ComboState.Charging01;
                animator.SetBool(Attack01_Charging, true);
                break;
        }
    }
    IEnumerator CheckIfHold()
    {
        float timer = 0;
        while (Input.GetKey(KeyCode.N))
        {
            timer += Time.deltaTime;
            if (timer > CheckIfHoldTime)
            {
                CheckCharging(); //Change to ActuallyCharging depending on which charge
            }
            yield return null;
        }
    }
    void CheckCharging()
    {
        if (comboState == ComboState.Charging01) { comboState = ComboState.ActuallyCharging01; }
        if (comboState == ComboState.Charging02) { comboState = ComboState.ActuallyCharging02; }
    }
    
    public void EndAttack01()
    {

        animator.SetBool(Attack01_Release, false);
        animator.SetBool(Attack01_Charging, false);
        
        ComboCue--;

    }
    public void EndAttack02()
    {

        animator.SetBool(Attack02_Release, false);
        animator.SetBool(Attack02_Charging, false);
        
        ComboCue--;
    }
    public void ComboStateNoAttack()
    {
        comboState = ComboState.NoAttacking;
    }
   
}

