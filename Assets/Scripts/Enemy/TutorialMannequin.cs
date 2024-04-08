using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMannequin : MonoBehaviour
{
    [SerializeField] bool isAttackingMannequin;
    private void Awake()
    {
        if(isAttackingMannequin)
        {
            gameObject.GetComponent<Animator>().SetBool("AttackingMannequin", true);
        }
        
    }
}
